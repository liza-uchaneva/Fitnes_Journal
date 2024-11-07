<template>
    <div>
        {{ profileId }}
    </div>

    <div class="calendar-week">
        <h2>{{ monthTitle }}</h2>
        <div class="calendar-days">
            <div v-for="(day, index) in currentWeek"
                 :key="index"
                 class="calendar-day"
                 :class="{ 'current-day': isToday(day) }">
                <span>{{ day.format("ddd") }}</span>
                <span>{{ day.format("D") }}</span>
            </div>
        </div>
    </div>
    <button @click="AddWorkout">Add workout</button>
</template>

<script>
    import axios from 'axios';
    import Day from './Day.vue';
    import dayjs from 'dayjs';

    export default {
        data() {
            return {
                profileId: null,
                workouts: [],
                currentWeek: [],
                monthTitle: '',
            }
        },
        async mounted() {
            await this.getProfileId();
            await this.loadWorkouts();
            this.getCurrentWeek();
        },

        methods: {
            isToday(day) {
                return day.isSame(dayjs(), 'day');
            },
            getCurrentWeek() {
                const today = dayjs();
                const startOfWeek = today.startOf('week').day(0);
                this.currentWeek = Array.from({ length: 7 }, (_, i) =>
                    startOfWeek.add(i, 'day')
                );

                this.monthTitle = today.format('MMMM');
            },
           
            async getProfileId() {
                try {
                    const response = await fetch(`/api/user/profileId`, {
                        method: 'GET',
                        headers: {
                            'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
                        }
                    });

                    if (response.ok) {
                        const data = await response.json();
                        console.log('ProfileId:', data);

                        this.profileId = data;
                    } else {
                        console.error('Failed to fetch profile:', response.statusText);
                    }
                } catch (error) {
                    console.error('Error:', error);
                }
            },

            async loadWorkouts() {
                try {
                    const response = await fetch(`/${this.profileId}/workouts`, {
                        method: 'GET',
                        headers: {
                            'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
                        }
                    });

                    if (response.ok) {
                        const responseText = await response.text();
                        console.log('Raw response text:', responseText);

                        const data = JSON.parse(responseText);
                        console.log('Workouts:', data);

                        this.workoutsd = data;
                    } else {
                        console.error('Failed to fetch workouts:', response.statusText);
                    }
                } catch (error) {
                    console.error('Error:', error);
                }
            },
            async AddWorkout() {
                try {
                    const response = await fetch(`/${this.profileId}/workouts`, {
                        method: 'POST',
                        headers: {
                            'Authorization': 'Bearer ' + localStorage.getItem('accessToken'),
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            ProfileId: this.profileId,
                            WorkoutDateTime: dayjs().toISOString()
                        })
                    });

                    if (response.ok) {
                        const data = await response;
                        console.log('WorkoutId:', data);
                        loadWorkouts();
                    } else {
                        console.error('Failed to post workout:', response.statusText);
                    }
                }
                catch (error) {
                    console.error('Error:', error);
                }
            },
        }
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
</style>


