<template>
  <section class="center">
    <div>
      <h1>Hi <span>{{ name }}</span></h1>
    </div>
    <div class="progress-container">
      <div
          id="bar"
          role="progressbar"
          :aria-valuenow="{ weekProgressInProcents }"
          aria-valuemin="0"
          :aria-valuemax="100"
          :style="{ '--value': weekProgressInProcents }"
      ></div>
      
    </div>
    <div class="calendar-week">
      <h2>{{ monthTitle }}</h2>
      <div class="calendar-days">
        <div v-for="(day, index) in currentWeek"
             :key="index"
             class="calendar-day"
             :class="{
                    'current-day': isToday(day),
                    'workout-day': isWorkoutDay(day),
                    'selected-day': day.isSame(selectedDay, 'day')
                }"
             @click="selectDay(day)">
          <span>{{ day.format("ddd") }}</span>
          <span>{{ day.format("D") }}</span>
        </div>
      </div>
      <button class="form__button" @click="this.postWorkout()">Add workout</button>
    </div>
    <GoalSetUp v-if="isGoalSet" @goal-set="hideGoalSetUp"></GoalSetUp>
  </section>
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
      name: '',
      workouts: [],
      checkedDays: [],
      currentWeek: [],
      selectedDay: null,
      goal: 7,
      weekProgress: 0,
      weekProgressInProcents: 0,
      monthTitle: '',
      isGoalSet: false,
    };
  },
  async mounted() {
    await this.getWeeklyGoal();
    await this.getUserName();
    await this.getProfileId();
    await this.getWorkouts();
    this.setCurrentWeek();
  },
  methods: {
    hideGoalSetUp() {
      this.isGoalSet = false;
      this.getWeeklyGoal();
    },
    
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
        this.weekProgressInProcents = this.weekProgress * 100 / this.goal;
      }
      return res;
    },

    selectDay(day) {
      if (day <= dayjs()) {
        this.selectedDay = day;
      }
    },
    setCurrentWeek() {
      const today = dayjs();
      const startOfWeek = today.startOf('week').day(0);
      this.currentWeek = Array.from({length: 7}, (_, i) =>
          startOfWeek.add(i, 'day')
      );

      this.monthTitle = today.format('MMMM');
    },
    
    async getUserName() {
      try {
        const response = await fetch(`/api/user/name`, {
          method: 'GET',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
          },
        });

        if (response.ok) {
          const data = await response.json();
          this.name = JSON.parse(data);
        } else {
          console.error('Failed to fetch name:', response.statusText);
          if (response.statusText === 'Unauthorized') {
            this.$router.replace({path: '/'});
          }
        }
      } catch (error) {
        console.error('Error:', error);
      }
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
          if (response.status === 204) {
            this.isGoalSet = true;
            return;
          }

          const data = await response.json();
          this.goal = JSON.parse(data);
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
    async getWorkouts() {
      try {
        const response = await fetch(`/api/user/workouts`, {
          method: 'GET',
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
          },
        });

        if (response.ok) {
          const responseText = await response.text();
          this.workouts = JSON.parse(responseText);
        } else {
          console.error('Failed to fetch workouts:', response.statusText);
        }
      } catch (error) {
        console.error('Error:', error);
      }
    },

    async postWorkout() {
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
        await this.loadWorkouts();
      } catch (error) {
        console.error('Error:', error);
      }
    },
  },
};
</script>