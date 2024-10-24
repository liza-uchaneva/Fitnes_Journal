<template>
    <div><h2>Main Page</h2></div>
    <div>
        {{ profileId }}
    </div>
    <div>
        {{ workouts }}
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        data() {
            return {
                profileId: null,
                workouts: [],
                year: new Date().getFullYear(),
                month: new Date().getMonth(),
            }
        },
        async mounted() {
            await this.getProfileId();
            await this.loadWorkouts();
        },

        methods: {
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
                    if (this.profileId) {
                        const response = await axios.get(`/${this.profileId}/workouts`);

                        this.workouts = response.json();
                    } else {
                        console.error('ProfileId is not set. Cannot load workouts.');
                    }
                } catch (error) {
                    console.error("Error loading workouts:", error);
                }
            },
        }
    };
</script>



