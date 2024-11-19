import { createRouter, createWebHistory } from "vue-router";
import Onboarding from "../pages/OnboardingPage.vue";
import Login from "../pages/Login.vue";
import SignUp from "../pages/SignUp.vue";

const routes = [
  {
    path: "/",
    name: "onboarding",
    component: Onboarding,
  },
  {
    path: "/login",
    name: "login",
    component: Login,
  },
  {
    path: "/signup",
    name: "signup",
    component: SignUp,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
