import { createRouter, createWebHistory } from 'vue-router'
import Login from '../pages/Login.vue'
import SignUp from '../pages/SignUp.vue'
import Main from '../pages/Main.vue'




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
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
});

export default router;
