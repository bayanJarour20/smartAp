import Vue from 'vue'
import bootstrapVue from 'bootstrap-vue'
import VueCompositionAPI from '@vue/composition-api'


import router from './router'
import store from './store'
import App from './App.vue'
import numeral from 'numeral';
import numFormat from 'vue-filter-number-format';

// Global Components
import './global-components'

// 3rd party plugins
import '@/libs/portal-vue'
import '@/libs/toastification'

// BSV Plugin Registration
Vue.use(bootstrapVue)
Vue.filter('numFormat', numFormat(numeral));

// Composition API
Vue.use(VueCompositionAPI)

// import core styles
require('@core/scss/core.scss')

// import assets styles
require('@/assets/scss/style.scss')
require('@core/scss/vue/libs/vue-select.scss')

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')
