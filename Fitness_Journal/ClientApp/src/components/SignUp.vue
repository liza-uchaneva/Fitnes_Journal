<template>
  <form class="center form" @submit.prevent="register">
    <h3 class="form__title">Create account</h3>

    <div class="form__input_box">
      <label class="form__label"></label>
      <input
        placeholder="Email"
        v-model.trim="user.email"
        :class="{ form__input_hasError: v$.email.$invalid && v$.email.$dirty }"
        class="form__input_signUp"
        type="text"
        name="email"
      />
      <p
        class="form__error_message"
        v-for="err in v$.email.$errors"
        :key="err.$uid"
      >
        *{{ err.$message }}
      </p>
    </div>

    <div class="form__input_box">
      <label class="form__label"></label>
      <input
        placeholder="Password"
        v-model.trim="user.password"
        :class="{
          form__input_hasError: v$.password.$invalid && v$.password.$dirty,
        }"
        class="form__input_signUp"
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
      <label class="form__label"></label>
      <input
        placeholder="Confirm password"
        v-model.trim="user.confirm_password"
        :class="{
          form__input_hasError:
            v$.confirm_password.$invalid && v$.confirm_password.$dirty,
        }"
        class="form__input_signUp"
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
    Already have an account? <router-link to="/login">Log in here</router-link>
  </div>
</template>

<script>
import axios from "axios";
import { reactive, computed } from "vue";
import { email, required, sameAs, minLength } from "@vuelidate/validators";
import { useVuelidate } from "@vuelidate/core";

export default {
  setup() {
    const user = reactive({
      email: "",
      password: "",
      confirm_password: "",
    });

    const rules = computed(() => ({
      email: { required, email },
      password: { required, minLength: minLength(6) },
      confirm_password: { required, sameAs: sameAs(user.password) },
    }));

    const v$ = useVuelidate(rules, user);

    const visible = reactive({ value: false });

    const resetForm = () => {
      user.email = "";
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
