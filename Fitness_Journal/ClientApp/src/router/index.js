import { createRouter, createWebHistory } from "vue-router";
import Onboarding from "../pages/OnboardingPage.vue";
import Login from "../pages/Login.vue";
import Main from '../pages/Main.vue';
import forgotPassword from "../pages/ForgotPasswordPage.vue";
import SignUp from "../pages/Signup.vue";
import Calendar from "../pages/Calendar.vue";




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
        path: "/calendar",
        name: "calendar",
        component: Calendar,
    },
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
});

export default router;
