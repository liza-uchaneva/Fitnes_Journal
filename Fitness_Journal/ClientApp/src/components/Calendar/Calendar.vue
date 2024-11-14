<template>
    <div>
        {{ profileId }}
    </div>
    <div>
        {{ workouts }}
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
    </div>
    <button @click="AddWorkout(day)">Add workout</button>
</template>

<script>
    import axios from 'axios';
    import dayjs from 'dayjs';

    export default {
        data() {
            return {
                profileId: null,
                workouts: [],
                currentWeek: [],
                monthTitle: '',
                selectedDay: null,
            };
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
            isWorkoutDay(day) {
                return this.workouts.some(workout =>
                    day.isSame(dayjs(workout), 'day')
                );
            },
            selectDay(day) {
                this.selectedDay = day;
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
                            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
                        },
                    });

                    if (response.ok) {
                        const data = await response.json();
                        this.profileId = data;
                    } else {
                        console.error('Failed to fetch profile:', response.statusText);
                        if (response.statusText === 'Unauthorized') {
                            this.$router.replace({ path: '/' });
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
                        profileId: this.profileId,
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
        background-color: #ffcccb;
    }

    .selected-day {
        border: 2px solid blue;
    }
</style>



