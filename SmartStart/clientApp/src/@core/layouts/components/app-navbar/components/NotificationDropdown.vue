<template>
  <b-nav-item-dropdown
    class="dropdown-notification mr-25"
    menu-class="dropdown-menu-media"
    left
  >
    <template #button-content>
      <b-badge
          variant="danger" 
          class="rounded-circle position-relative" 
          style="top: -8px; right: 12px">
          {{'4'}}
      </b-badge>

      <unicon
        wdith="18"
        :fill="`${isDark ? '#fff' : '#5E5873'}`"
        name="bell"
      />
    </template>
    
    <!-- Notifications -->
    <vue-perfect-scrollbar
      v-if="notificationsDash.length > 0"
      v-once
      class="scrollable-container media-list scroll-area"
      tagname="li"
    >
      <b-link
        v-for="notification in notificationsDash"
        :key="notification.id"
      >
        <b-media @click="notification.posId != null ? goto(notification.posId) : ''">
          <p class="media-heading" style="display: flex; justify-content: space-between;">
            <span class="font-weight-bolder">
              {{ notification.message }}
            </span>
            {{notification.isRead}}
            <b-badge
               v-if="!notification.isRead && notificationCount"
               variant="danger" 
               class="rounded-circle position-relative" 
               >
            </b-badge>
          </p>
        </b-media>
      </b-link>
    </vue-perfect-scrollbar>


    <!-- Cart Footer -->
    <li class="dropdown-menu-footer" v-if="notificationsDash.length > 0">
      <b-button
      v-ripple.400="'rgba(255, 255, 255, 0.15)'"
      variant="primary"
      block
      >تمييز كمقروء</b-button>
    </li>
    <div v-else class="p-1">
      لا توجد إشعارات جديدة
    </div>
  </b-nav-item-dropdown>
</template>

<script>
import {
  BNavItemDropdown, BBadge, BMedia, BLink,  BButton, 
} from 'bootstrap-vue'
import VuePerfectScrollbar from 'vue-perfect-scrollbar'
import Ripple from 'vue-ripple-directive'
import useAppConfig from '@core/app-config/useAppConfig'
import { computed } from '@vue/composition-api'
export default {
  components: {
    BNavItemDropdown,
    BBadge,
    BMedia,
    BLink,
    VuePerfectScrollbar,
    BButton,
  },
  directives: {
    Ripple,
  },
  setup() {
    const { skin } = useAppConfig()
    const isDark = computed(() => skin.value === 'dark')
    return {
      isDark,
    } 
  },
data: () => ({
    notificationsDash: []
}),
  computed:{
  },
  methods:{
  },
  created(){
  },
}
</script>
