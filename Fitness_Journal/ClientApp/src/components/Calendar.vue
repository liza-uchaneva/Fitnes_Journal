<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <section class="center">
    <div class = "total">
      <h1>{{ workouts.length }}</h1>
      <p>Total workouts</p>
    </div>
    <div class="calendar">
      <div class="calendar-month">
        <div class="month-slider">
          <button @click="changeMonth(-1)" class="slider-button">‹</button>
          <h2>{{ monthTitle }}</h2>
          <button @click="changeMonth(1)" class="slider-button">›</button>
        </div>
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
    </div>
  </section>
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
      currentDate: dayjs(),
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
      const startOfMonth = this.currentDate.startOf("month");
      const daysInMonth = this.currentDate.daysInMonth();
      this.firstDayOffset = (startOfMonth.day() + 6) % 7;

      this.currentMonth = Array.from({length: daysInMonth}, (_, i) =>
          startOfMonth.add(i, "day")
      );
      this.monthTitle = this.currentDate.format("MMMM YYYY");
    },
    changeMonth(offset) {
      this.currentDate = this.currentDate.add(offset, "month");
      this.getCurrentMonth();
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
            profileId: localStorage.getItem("profileId"),
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
.calendar {
  background: #ffffff;
}

.calendar-month {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 20px;
  padding: 20px;
}

.month-slider {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
}

.slider-button {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
}

.calendar-grid {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  width: 100%;
}

.calendar-header {
  font-weight: bold;
  text-align: center;
  padding: 5px 0;
}

.total{
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
</style>




