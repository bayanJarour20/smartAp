import Vue from 'vue'
import Vuex from 'vuex'

// Modules
import app from './app'
import appConfig from './app-config'
import verticalMenu from './vertical-menu'

import university from './global-store/university'
import filter from './global-store/filter'
import globalStore from './global-store'

import home from "./home" 


Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    app,
    appConfig,
    verticalMenu,
    home,

    filter,
    globalStore,
    university,
  },
  strict: process.env.DEV,
})
