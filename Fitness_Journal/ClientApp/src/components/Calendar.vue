<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <div class="calendar-month">
    <h2>{{ monthTitle }}</h2>
    <div class="calendar-grid">
      <div v-for="(weekday, index) in weekdays" :key="'header-' + index" class="calendar-header">
        {{ weekday }}
      </div>
      <div v-for="n in firstDayOffset" :key="'placeholder-' + n" class="calendar-placeholder"></div>
      <div
          v-for="(day, index) in currentMonth"
          :key="index"
          class="calendar-day"
          :class="{
          'current-day': isToday(day),
          'workout-day': isWorkoutDay(day),
          'selected-day': day.isSame(selectedDay, 'day')
        }"
          @click="selectDay(day)"
      >
        {{ day.format('D') }}
      </div>
    </div>
    <button class="form__button" @click="addWorkout">Add Workout</button>
  </div>
</template>

<script>
import dayjs from "dayjs";

export default {
  data() {
    return {
      workouts: [],
      currentMonth: [],
      selectedDay: null,
      monthTitle: "",
      weekdays: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
      firstDayOffset: 0,
    };
  },
  mounted() {
    this.loadWorkouts();
    this.getCurrentMonth();
  },
  methods: {
    isToday(day) {
      return day.isSame(dayjs(), "day");
    },
    isWorkoutDay(day) {
      return this.workouts.some((workout) => day.isSame(dayjs(workout), "day"));
    },
    selectDay(day) {
      if (day.isBefore(dayjs().endOf("day"))) {
        this.selectedDay = day;
      }
    },
    getCurrentMonth() {
      const startOfMonth = dayjs().startOf("month");
      const daysInMonth = dayjs().daysInMonth();
      this.firstDayOffset = (startOfMonth.day() + 6) % 7; 

      this.currentMonth = Array.from({ length: daysInMonth }, (_, i) =>
          startOfMonth.add(i, "day")
      );
      this.monthTitle = dayjs().format("MMMM YYYY");
    },
    async loadWorkouts() {
      try {
        const response = await fetch("/api/user/workouts", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          },
        });
        if (response.ok) {
          this.workouts = await response.json();
        } else {
          console.error("Failed to load workouts:", response.statusText);
        }
      } catch (error) {
        console.error("Error loading workouts:", error);
      }
    },
    async addWorkout() {
      if (!this.selectedDay) return;
      try {
        const response = await fetch("/api/user/workout", {
          method: "POST",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            workoutDateTime: this.selectedDay.format(),
          }),
        });
        if (response.ok) {
          await this.loadWorkouts();
        } else {
          console.error("Failed to add workout:", response.statusText);
        }
      } catch (error) {
        console.error("Error adding workout:", error);
      }
    },
  },
};
</script>

<style scoped>
.calendar-month {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 20px;
}
.calendar-grid {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 5px;
  width: 100%;
}
.calendar-header {
  font-weight: bold;
  text-align: center;
  padding: 5px 0;
}
.calendar-placeholder {
  visibility: hidden;
}
.calendar-day {
  padding: 10px;
  text-align: center;
  border: 1px solid #ddd;
  border-radius: 4px;
}
.current-day {
  font-weight: bold;
  border: 2px solid black;
}
.workout-day {
  background-color: #f0f8ff;
}
.selected-day {
  border: 2px solid blue;
}
</style>



