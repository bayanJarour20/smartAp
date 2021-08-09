import Vue from 'vue'
import Vuex from 'vuex'

// Modules
import app from './app'
import appConfig from './app-config'
import verticalMenu from './vertical-menu'

import filter from './global-store/filter'
import globalStore from './global-store'

import faculties from "./content-management/faculties"
import advertising from './content-management/advertising'

import home from "./home" 

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    app,
    appConfig,
    verticalMenu,
    home,
    advertising,
    faculties,
   
    filter,
    globalStore,
  },
  strict: process.env.DEV,
})
