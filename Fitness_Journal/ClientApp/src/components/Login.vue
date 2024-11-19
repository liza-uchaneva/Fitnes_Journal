<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <section class="center login">
    <form @submit.prevent="onSend" class="form">
      <h3 class="form__title">Welcome back</h3>
      <div class="form__input_box">
        <label class="form__label"></label>
        <input
          placeholder="Email"
          v-model.trim="user.email"
          :class="{
            form__input_hasError: v$.email.$invalid && v$.email.$dirty,
          }"
          class="form__input"
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
      <button type="submit" class="form__button">Submit</button>
    </form>
    <img
      class="login__img"
      src="../assets/picture/login.webp"
      alt="active people"
    />
    <div>
      New to us?
      <router-link to="/signup">Sign up now</router-link>
    </div>
  </section>
</template>

<script>
import axios from "axios";
import { reactive, computed } from "vue";
import { email, required } from "@vuelidate/validators";
import { useVuelidate } from "@vuelidate/core";

export default {
  setup() {
    const user = reactive({
      email: "",
      password: "",
    });

    const rules = computed(() => ({
      email: { required, email },
      password: { required },
    }));

    const v$ = useVuelidate(rules, user);

    const visible = reactive({ value: false });

    const resetForm = () => {
      user.email = "";
      user.password = "";
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
          await axios.post("api/login", {
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
