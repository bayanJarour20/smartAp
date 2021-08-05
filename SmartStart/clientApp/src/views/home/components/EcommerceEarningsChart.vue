<template>
  <b-card
    v-if="data"
    class="earnings-card"
  >
    <b-row>
      <b-col cols="6">
        <b-card-title class="mb-1">
          الأكواد المولدة ({{data.allCodeCount}})
          <!-- {{ data.title }} -->
        </b-card-title>
        <div class="font-small-2">
         قيمة {{ data.title }} هذا الشهر.
        </div>
        <h5 class="mb-1">
          {{ data.total }} ل.س 
        </h5>
        <b-card-text class="text-muted font-small-2">
          <span class="font-weight-bolder">{{ data.compareRate >= 0 ? data.compareRate * 100
                                                                    : -1 * data.compareRate * 100 }}%</span>
          <span>{{ data.compareRate >= 0 ? ' أكثر' : ' أقل'}}</span>
          <span> من الشهر الفائت.</span>
        </b-card-text>
      </b-col>
      <b-col cols="6">
        <!-- chart -->
        <vue-apex-charts
         class="donat"
          :options="earningsChart.chartOptions"
          :series="[ data.allCodeCount - data.codeCount , data.codeCount == 0 ? 1 : data.codeCount]"
        />
      </b-col>
    </b-row>
  </b-card>
</template>
<style lang="scss">
.donat{
.apexcharts-datalabels-group{
  display: none;
}
}
</style>
<script>
import {
  BCard, BRow, BCol, BCardTitle, BCardText,
} from 'bootstrap-vue'
import VueApexCharts from 'vue-apexcharts'
import { $themeColors } from '@themeConfig'

export default {
  components: {
    BCard,
    BRow,
    BCol,
    BCardTitle,
    BCardText,
    VueApexCharts,
  },
  props: {
    data: {
      type: Object,
      default: () => {},
    },
  },
  data() {
    return {
      earningsChart: {
        chartOptions: {
          chart: {
            type: 'donut',
            toolbar: {
              show: false,
            },
          },
          dataLabels: {
            enabled: false,
          },
          legend: { show: false },
          labels: [`الأكواد المتبقية`, `${this.data.title}`],
          stroke: { width: 0 },
          colors: [ '#fcb26a', $themeColors.success],
          grid: {
            padding: {
              right: -20,
              bottom: -8,
              left: -20,
            },
          },
          plotOptions: {
            pie: {
              startAngle: -10,
              donut: {
                labels: {
                  show: true,
                  name: {
                    offsetY: 15,
                  },
                  value: {
                    offsetY: -15,
                    formatter() {
                      // eslint-disable-next-line radix
                      return ``
                    },
                  },
                  total: {
                    show: true,
                    offsetY: 15,
                    // label: '',
                    formatter() {
                      return '100'
                    },
                  },
                },
              },
            },
          },
          responsive: [
            {
              breakpoint: 1325,
              options: {
                chart: {
                  height: 100,
                },
              },
            },
            {
              breakpoint: 1200,
              options: {
                chart: {
                  height: 120,
                },
              },
            },
            {
              breakpoint: 1045,
              options: {
                chart: {
                  height: 100,
                },
              },
            },
            {
              breakpoint: 992,
              options: {
                chart: {
                  height: 120,
                },
              },
            },
          ],
        },
      },
    }
  },
}
</script>
