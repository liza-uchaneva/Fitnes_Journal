import { createRouter, createWebHistory } from 'vue-router'
import Login from '../pages/Login.vue'
import SignUp from '../pages/SignUp.vue'
import Main from '../pages/Main.vue'
import Personal_info from '../pages/Personal_information.vue'




const routes = [
    {
        path: "/",
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
