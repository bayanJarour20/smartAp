import Vue from 'vue'
import { $themeConfig } from '@themeConfig'

// axios
import axios from 'axios'

const axiosIns = axios.create({
  // You can add your headers here
  // ================================
  baseURL: $themeConfig.app.domainPath + '/api/',
  // timeout: 1000,
//   headers: {'X-Custom-Header': 'foobar'}
})
axiosIns.CancelToken = axios.CancelToken
Vue.prototype.$http = axiosIns

export default axiosIns
