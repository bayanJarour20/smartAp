<template>
  <b-card no-body>
    <b-card-body class="pb-4">
      <b-avatar
        :variant="`light-${color}`"
        size="45"
      >
        <unicon :name="icon" width="22" height="22" />
      </b-avatar>
    <span class="pl-2">{{ statisticTitle }}</span>
    </b-card-body>

    <vue-apex-charts
      type="area"
      height="100"
      width="100%"
      :options="chartOptionsComputed"
      :series="chartData"
    />

  </b-card>
</template>

<script>
import { BCard, BCardBody, BAvatar } from 'bootstrap-vue'
import VueApexCharts from 'vue-apexcharts'
import { $themeColors } from '@themeConfig'
import { areaChartOptions } from './chartOptions'

export default {
  components: {
    VueApexCharts,
    BCard,
    BCardBody,
    BAvatar,
  },
  props: {
    icon: {
      type: String,
      default: () => 'user'
    },
    statistic: {
      type: Number,
    },
    statisticTitle: {
      type: String,
      default: '',
    },
    color: {
      type: String,
      default: 'warning',
    },
    chartData: {
      type: Array,
      default: () => [],
    },
    chartOptions: {
      type: Object,
      default: null,
    },
  },
  computed: {
    chartOptionsComputed() {
      if (this.chartOptions === null) {
        const options = JSON.parse(JSON.stringify(areaChartOptions))
        options.theme.monochrome.color = $themeColors[this.color]
        return options
      }
      return this.chartOptions
    },
  },
}
</script>
