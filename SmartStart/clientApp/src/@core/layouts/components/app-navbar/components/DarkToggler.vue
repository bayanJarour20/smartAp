<template>
  <b-nav-item @click="skin = isDark ? 'light' : 'dark'">
    <unicon
      wdith="18"
      :fill="`${isDark ? '#fff' : '#5E5873'}`"
      :name="`${isDark ? 'sun' : 'moon'}`"
    />
  </b-nav-item>
</template>

<script>
import useAppConfig from '@core/app-config/useAppConfig'
import { computed } from '@vue/composition-api'
import { BNavItem } from 'bootstrap-vue'

export default {
  components: {
    BNavItem,
  },
  setup() {
    const { skin } = useAppConfig()

    const isDark = computed(() => skin.value === 'dark')

    return { skin, isDark }
  },
}
</script>
