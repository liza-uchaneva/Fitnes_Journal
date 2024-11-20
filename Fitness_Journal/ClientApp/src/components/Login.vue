<!-- eslint-disable vue/multi-word-component-names -->
<template>
    <form @submit.prevent="onSend">
        <h3 class="form__title">Welcome back</h3>
        <div class="form__input_box">
            <label class="form__label">Email </label>
            <input v-model.trim="user.email"
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
            <label class="form__label">Password </label>
            <input v-model.trim="user.password"
                   :class="{ 'form__input_hasError': (v$.password.$invalid && v$.password.$dirty) }"
                   class="form__input"
                   type="password"
                   name="password" />
            <p class="form__error_message"
               v-for="err in v$.password.$errors"
               :key="err.$uid">
                *{{ err.$message }}
            </p>
        </div>
        <button type="submit" class="form__button">Submit</button>
    </form>
    <div>
        New to us? <router-link to="/signup">Sign up</router-link> now
    </div>
</template>

<script>
    import axios from 'axios';
    import { reactive, computed } from 'vue';
    import { email, required, maxLength } from '@vuelidate/validators';
    import { useVuelidate } from '@vuelidate/core';

    export default {
        setup() {
            const user = reactive({
                email: '',
                password: '',
            });

            const rules = computed(() => ({
                email: { required, email },
                password: { required, maxLength }
            }));

            const v$ = useVuelidate(rules, user);

            const visible = reactive({ value: false });

            const resetForm = () => {
                user.email = '';
                user.password = '';
            };
            const mes = "The password must be at least six characters long and contain at least one of each of the following characters: \br  - Uppercase letter \br  - Lowercase letter \br - Numeric digit \br - Nonalphanumeric character";
            return {
                mes,
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
                       const responce = await axios.post('api/login', {
                            email: this.user.email,
                            password: this.user.password,
                        });
                        localStorage.setItem('accessToken', responce.data.accessToken);

                        this.$router.replace({ path: '/home' });
        } catch (error) {
                        const er = error.response + this.mes;
                        console.error('Error:', er);
        }
        this.visible.value = true;
        this.resetForm();
        this.v$.$reset();
      }
    },
  },
};
</script>
