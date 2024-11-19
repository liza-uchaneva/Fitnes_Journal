<template>
    <form @submit.prevent="SetWeeklyGoal">
        <h3 class="form__title">Almost there!</h3>
        <h4>Commit weekly goal to make steady progress</h4>
        <div class="form__input_goal">
            <input type="range"
                   min="1"
                   max="7"
                   v-model="weeklyGoal"
                   class="slider"
                   id="goalRange">
            <span class="range-value">{{ weeklyGoal }} workouts</span>
        </div>
        <p class="goal-message">{{ goalMessage }}</p>
        <button type="submit" class="form__button">Complete</button>
    </form>
</template>

<script>
    import axios from 'axios';
    import { reactive, computed, ref } from 'vue';

    export default {
        setup() {
            const weeklyGoal = ref(3);

            const goalMessage = computed(() => {
                if (weeklyGoal.value < 3) {
                    return "A little goes a long way! Let's keep it steady.";
                } else if (weeklyGoal.value <= 5) {
                    return "Great choice! Consistency is key to progress.";
                } else {
                    return "You're on fire! Aim high and achieve more!";
                }
            });

            return {
                weeklyGoal,
                goalMessage,
            };
        },

        methods: {
            async SetWeeklyGoal() {
                try {
                    const config = {
                        headers: {
                            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
                            'Content-Type': 'application/json',
                        },
                    };

                    const bodyParameters = {
                        profileId: localStorage.getItem('profileId'),
                        weeklygoal: this.weeklyGoal,
                    };

                    const response = await axios.post(`/api/user/setweeklygoal`, bodyParameters, config);

                    this.$router.replace({ path: '/home' });
                } catch (error) {
                    const er = error.response;
                    console.error('Error:', er);
                }
            },
        },
    };
</script>
<style scoped>
    .form__input_goal {
        display: flex;
        flex-direction: column;
    }
</style>


