import { createRouter, createWebHistory } from "vue-router";
import Onboarding from "../pages/OnboardingPage.vue";
import Login from "../pages/Login.vue";
import Main from '../pages/Main.vue'


const routes = [
  {
    path: "/",
    name: "onboarding",
    component: Onboarding,
  },
  {
    path: "/forgotPassword",
    name: "forgotPassword",
    component: forgotPassword,
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
    {
        path: "/home",
        name: "home",
        component: Main,
    },
    {
        path: "/personal_info",
        name: "personal_info",
        component: Personal_info,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
