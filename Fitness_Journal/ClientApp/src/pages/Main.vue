<template>
  <Home/>
  <Navigation/>
</template>

<script>
import Home from "../components/HomePage/Home.vue";
import Navigation from "@/components/Navigation.vue";


export default {
  components: {
    Home,
    Navigation,
  },
};
</script>

<style lang="scss" scoped></style>
