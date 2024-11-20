<template>
  <div class="progress-container">
    <span>{{ weekProgress }} / {{ goal }}</span>

    <progress id="bar" :max="goal" :value="weekProgress"></progress>
  </div>
  <div class="calendar-week">
    <h2>{{ monthTitle }}</h2>
    <div class="calendar-days">
      <div v-for="(day, index) in currentWeek"
           :key="index"
           class="calendar-day"
           :class="{
                    'current-day': isToday(day),
                    'current-day': isToday(day),
                    'workout-day': isWorkoutDay(day),
                    'selected-day': day.isSame(selectedDay, 'day')
                }"
           @click="selectDay(day)">
        <span>{{ day.format("ddd") }}</span>
        <span>{{ day.format("D") }}</span>
      </div>
    </div>
  </div>
  <button @click="AddWorkout()">Add workout</button>
  <GoalSetUp v-if="isGoalSet"></GoalSetUp>
</template>

<script>
import axios from 'axios';
import dayjs from 'dayjs';
import GoalSetUp from './GoalSetUp.vue'

export default {
  components: {
    GoalSetUp
  },
  data() {
    return {
      profileId: null,
      workouts: [],
      checkedDays: [],
      currentWeek: [],
      selectedDay: null,
      goal: 7,
      weekProgress: 0,
      monthTitle: '',
      isGoalSet: false,
    };
  },
  async mounted() {
    await this.getWeeklyGoal();

    await this.getProfileId();
    await this.loadWorkouts();
    this.getCurrentWeek();
  },
  methods: {

    isToday(day) {
      return day.isSame(dayjs(), 'day');
    },

    isWorkoutDay(day) {
      const res = this.workouts.some(workout =>
          day.isSame(dayjs(workout), 'day')
      );
      if (res && !this.checkedDays.includes(day.format('YYYY-MM-DD'))) {
        this.weekProgress++;
        this.checkedDays.push(day.format('YYYY-MM-DD'));
      }
      return res;
    },

    selectDay(day) {
      if (day <= dayjs()) {
        this.selectedDay = day;
      }
    },

    getCurrentWeek() {
      const today = dayjs();
      const startOfWeek = today.startOf('week').day(0);
      this.currentWeek = Array.from({length: 7}, (_, i) =>
          startOfWeek.add(i, 'day')
      );

      this.monthTitle = today.format('MMMM');
    },

    async getProfileId() {
      try {
        const response = await fetch(`/api/user/profileId`, {
          method: 'GET',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
          },
        });

        if (response.ok) {
          const data = await response.json();
          localStorage.setItem('profileId', data);

        } else {
          console.error('Failed to fetch profile:', response.statusText);
          if (response.statusText === 'Unauthorized') {
            this.$router.replace({path: '/'});
          }
        }
      } catch (error) {
        console.error('Error:', error);
      }
    },
    async getWeeklyGoal() {
      try {
        const response = await fetch(`/api/user/weeklygoal`, {
          method: 'GET',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
          },
        });

        if (response.ok) {
          if (response.status === 204) this.isGoalSet = true;

          const data = await response.json();
          this.goal = data;
          if (data === 0) this.$router.replace({path: '/personal_info'});

        } else {
          console.error('Failed to fetch weekly goal:', response.statusText);
          if (response.statusText === 'Unauthorized') {
            this.$router.replace({path: '/'});
          }
        }
      } catch (error) {
        console.error('Error:', error);
      }
    },
    async loadWorkouts() {
      try {
        const response = await fetch(`/api/user/workouts`, {
          method: 'GET',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
          },
        });

        if (response.ok) {
          const responseText = await response.text();
          const data = JSON.parse(responseText);
          this.workouts = data;
        } else {
          console.error('Failed to fetch workouts:', response.statusText);
        }
      } catch (error) {
        console.error('Error:', error);
      }
    },

    async AddWorkout() {
      try {
        const config = {
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
            'Content-Type': 'application/json',
          },
        };

        const bodyParameters = {
          profileId: localStorage.getItem('profileId'),
          workoutDateTime: this.selectedDay,
        };

        const response = await axios.post(`/api/user/workout`, bodyParameters, config);
        this.loadWorkouts();
      } catch (error) {
        console.error('Error:', error);
      }
    },
  },
};
</script>

<style scoped>

.calendar-week {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.calendar-days {
  display: flex;
  gap: 10px;
}

.calendar-day {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  text-align: center;
  display: flex;
  flex-direction: column;
}

.current-day {
  color: #000;
  font-weight: bold;
  border: 2px solid black;
}

.workout-day {
  background-color: #F1F1F9;
}

.selected-day {
  border: 2px solid blue;
}

.progress-container {
  display: flex;
  align-items: center;
  gap: 10px;
  display: flex;
  flex-direction: column;
}

progress {
  width: 100%;
  height: 20px;
  appearance: none;
  border-radius: 5px;
  overflow: hidden;
}

progress::-webkit-progress-bar {
  background-color: #f3f3f3;
  border-radius: 5px;
}

progress::-webkit-progress-value {
  background-color: #4caf50;
  border-radius: 5px;
}

progress::-moz-progress-bar {
  background-color: #4caf50;
  border-radius: 5px;
}
</style>



