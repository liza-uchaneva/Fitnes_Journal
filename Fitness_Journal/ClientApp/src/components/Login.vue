<!-- eslint-disable vue/multi-word-component-names -->
<template>
    <form @submit.prevent="onSend">
        <h3 class="form__title">Login</h3>
        <div class="form__input_box">
            <label class="form__label">Email</label>
            <input v-model.trim="application.email"
                   :class="{ 'form__input_hasError': (v$.email.$invalid && v$.email.$dirty) }"
                   class="form__input"
                   type="text"
                   name="email" />
            <p class="form__error_message"
               v-for="err in v$.email.$errors"
               :key="err.$uid">
                *{{ err.$message }}
            </p>
        </div>
        <div class="form__input_box">
            <label class="form__label">Password</label>
            <input v-model.trim="application.password"
                   :class="{ 'form__input_hasError': (v$.password.$invalid && v$.password.$dirty) }"
                   class="form__input"
                   type="text"
                   name="password" />
            <p class="form__error_message"
               v-for="err in v$.password.$errors"
               :key="err.$uid">
                *{{ err.$message }}
            </p>
        </div>
        <button type="submit" class="form__button">Send</button>
    </form> 
</template>

<script>
    import axios from 'axios';
    import { reactive, computed } from 'vue';
    import { email, required } from '@vuelidate/validators';
    import { useVuelidate } from '@vuelidate/core';

    export default {
        setup() {
            const user = reactive({
                email: '',
                password:'',
            });

            const rules = computed(() => ({
                email: { required, email },
                password:{ required },
            }));

            const v$ = useVuelidate(rules, user);

            const visible = reactive({ value: false });

            const resetForm = () => {
                user.name = '';
                user.password = '';
            };

            return {
                user,
                v$,
                visible,
                resetForm,
            };
        },

        methods: {
            async onSend() {
                this.v$.$touch();
                if (!this.v$.$invalid) {
                    try {
                        await axios.post('api/Applications', {
                            name: this.user.name,
                            password: this.user.password,
                        });
                    } catch (error) {
                        console.error('Error:', error.response);
                    }
                    this.visible.value = true;
                    this.resetForm();
                    this.v$.$reset();
                }
            },
        }
    };
</script>
