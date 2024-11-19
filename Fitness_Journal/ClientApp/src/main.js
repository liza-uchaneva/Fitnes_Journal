import { createApp } from "vue";
import Vuelidate from "vuelidate";
import App from "./App.vue";
import router from "./router";
import "../src/assets/scss/style.scss";

createApp(App).use(Vuelidate).use(router).mount("#app");
