<!-- eslint-disable vue/multi-word-component-names -->
<template>
    <nav class="bottom-nav">
      <button @click="navigateTo('home')" class="nav-button">
        <span>🏠</span>
      </button>
      <button @click="navigateTo('calendar')" class="nav-button">
        <span>📅</span>
      </button>
    </nav>
</template>

<script>
export default {
  methods: {
    navigateTo(route) {
      this.$router.push({ name: route });
    },
  },
};
</script>

<style scoped>
.bottom-nav {
  position: fixed;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 60px;
  background: #f8f9fa;
  display: flex;
  justify-content: space-around;
  align-items: center;
  border-top: 1px solid #ddd;
  box-shadow: 0 -2px 5px rgba(0, 0, 0, 0.1);
}

.nav-button {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: none;
  border: none;
  font-size: 16px;
  cursor: pointer;
  color: #495057;
}

.nav-button span {
  font-size: 20px;
}

.nav-button:hover {
  color: #007bff;
}
</style>


