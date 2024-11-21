<template>
  <form class="center form" @submit.prevent="register">
    <h3 class="form__title">Create account</h3>

    <div class="form__input_box">
      <label class="form__label">Name </label>
      <input v-model.trim="user.username"
             :class="{ 'form__input_hasError': (v$.username.$invalid && v$.username.$dirty) }"
             class="form__input"
             type="text"
             name="username"/>
      <p class="form__error_message"
         v-for="err in v$.username.$errors"
         :key="err.$uid">
        *{{ err.$message }}
      </p>
    </div>

    <div class="form__input_box">
      <label class="form__label">Email </label>
      <input v-model.trim="user.email"
             :class="{ 'form__input_hasError': (v$.username.$invalid && v$.username.$dirty)}"
             class="form__input"
             type="text"
             name="email"/>
      <p
          class="form__error_message"
          v-for="err in v$.email.$errors"
          :key="err.$uid"
      >
        *{{ err.$message }}
      </p>
    </div>

    <div class="form__input_box">
      <label class="form__label">Password </label>
      <input
          v-model.trim="user.password"
          :class="{form__input_hasError: v$.password.$invalid && v$.password.$dirty}"
          class="form__input"
          type="password"
          name="password"
      />
      <p
          class="form__error_message"
          v-for="err in v$.password.$errors"
          :key="err.$uid"
      >
        *{{ err.$message }}
      </p>
    </div>
    <div class="form__input_box">
      <label class="form__label">Confirm password</label>
      <input
          v-model.trim="user.confirm_password"
          :class="{
          form__input_hasError:
            v$.confirm_password.$invalid && v$.confirm_password.$dirty,
        }"
          class="form__input"
          type="password"
          name="confirm_password"
      />
      <p
          class="form__error_message"
          v-for="err in v$.confirm_password.$errors"
          :key="err.$uid"
      >
        *{{ err.$message }}
      </p>
    </div>

    <button type="submit" class="form__button_submit">Submit</button>
  </form>

  <div class="center signUp__Wrap">
    Already have an account?
    <router-link to="/">Sign in here</router-link>
  </div>
</template>

<script>
import axios from "axios";
import {reactive, computed} from "vue";
import {email, required, sameAs, minLength} from "@vuelidate/validators";
import {useVuelidate} from "@vuelidate/core";

export default {
  setup() {
    const user = reactive({
      username: '',
      password: "",
      confirm_password: "",
    });

    const rules = computed(() => ({
      username: {required, minLength: minLength(2)},
      email: {required, email},
      password: {required, minLength: minLength(6)},
      confirm_password: {required, sameAs: sameAs(user.password)},
    }));

    const v$ = useVuelidate(rules,user);

    const visible = reactive({value: false});

    const resetForm = () => {
      user.username = '';
      user.password = "";
      user.confirm_password = "";
    };

    return {
      user,
      v$,
      visible,
      resetForm,
    };
  },

  methods: {
    async register() {
      this.v$.$touch();
      if (!this.v$.$invalid) {
        try {
          await axios.post("api/register", {
            email: this.user.email,
            password: this.user.password,
          });
          this.$router.replace({path: '/login'});
        } catch (error) {
          console.error("Error:", error.response);
        }
        this.visible.value = true;
        this.resetForm();
        this.v$.$reset();
      }
    },
  },
};
</script>
