<template>
  <b-navbar-nav class="nav flex-nowrap pl-1">
    <DarkToggler />
    <!-- <notification-dropdown /> -->
  </b-navbar-nav>
</template>

<script>
import {
  BNavbarNav,
} from 'bootstrap-vue'
// import NotificationDropdown from './components/NotificationDropdown.vue'
import DarkToggler from './components/DarkToggler.vue'

export default {
  components: {
    // Navbar Components
    BNavbarNav,
    DarkToggler,
    // NotificationDropdown,
  },
  props: {
    toggleVerticalMenuActive: {
      type: Function,
      default: () => {},
    },
  },
}
</script>
