<template>
    <div>
        <b-row>
            <b-col>
                <b-row>
                    <b-col lg="3" sm="6">
                        <statistic-card-horizontal
                            icon="horizontal-align-right"
                            :statistic="homeObj.subjectCount"
                            statistic-title="عدد المواد"
                        />
                    </b-col>
                    <b-col lg="3" sm="6">
                        <statistic-card-horizontal
                            icon="map"
                            color="success"
                            :statistic="homeObj.tagCount"
                            statistic-title="عدد الفصول"
                        />
                    </b-col>
                    <b-col lg="3" sm="6">
                        <statistic-card-horizontal
                            icon="server-alt"
                            color="danger"
                            :statistic="homeObj.examCount"
                            statistic-title="عدد الدورات"
                        />
                    </b-col>
                    <b-col lg="3" sm="6">
                        <statistic-card-horizontal
                            icon="newspaper"
                            color="warning"
                            :statistic="homeObj.interviewCount"
                            statistic-title="عدد الأسئلة النصية"
                        />
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
        <b-row>
            <b-col cols="12" md="4">
                <statistic-card-with-area-chart
                    v-if="sellersMonthlyCount"
                    color="primary"
                    icon="money-withdrawal"
                    statistic-title='المشتركين من نقاط البيع شهريا"'
                    :chartOptions="areaLineChart"
                    :chart-data="[
                        {
                            name: [''],
                            data: sellersMonthlyCount
                        }
                    ]"
                />
            </b-col>
            <b-col cols="12" md="4">
                <statistic-card-with-area-chart
                    v-if="usersMonthlyCount"
                    color="primary"      
                    icon="user"
                    statistic-title='المشتركين من الطلاب شهريا"'
                    :chartOptions="areaLineChart"
                    :chart-data="[
                        {
                            name: [''],
                            data: usersMonthlyCount
                        }
                    ]"
                />
            </b-col>
            <b-col cols="12" md="4">
                <statistic-card-horizontal
                    icon="layer-group"
                    :statistic="homeObj.bankCount"
                    statistic-title="عد البنوك"
                />
                <statistic-card-horizontal
                    icon="telescope"
                    :statistic="homeObj.microscopeCount"
                    statistic-title="عدد المجاهر"
                />
            </b-col>
            <b-col cols="12" md="8">
                <b-card
                    v-if="invoicesCount[selectedYear]"
                    no-body
                    class="card-revenue-budget"
                >
                    <b-row class="mx-0">
                        <b-col md="8" class="revenue-report-wrapper">
                            <div
                                class="d-sm-flex justify-content-between align-items-center mb-3"
                            >
                                <h4 class="card-title mb-50 mb-sm-0">
                                    الأرباح الشهرية
                                </h4>
                            </div>

                            <!-- chart -->
                            <vue-apex-charts
                                id="revenue-report-chart"
                                type="bar"
                                height="230"
                                :options="chartOptions"
                                :series="[
                                    {
                                        data: invoicesCount[selectedYear]
                                    }
                                ]"
                            />
                        </b-col>
                        <b-col md="4" class="budget-wrapper">
                            <b-dropdown
                                text="2020"
                                size="sm"
                                class="budget-dropdown"
                                variant="outline-primary"
                            >
                                <b-dropdown-item
                                    v-for="(year, index) in years"
                                    :key="index"
                                    @click="selectedYear = year.id"
                                >
                                    {{ year.name }}
                                </b-dropdown-item>
                            </b-dropdown>
                            <h3 class="mt-3 mb-2">الربح السنوي</h3>
                            <h4 class="mb-25">{{ yearEarnning }}</h4>
                        </b-col>
                    </b-row>
                </b-card>
            </b-col>
            <b-col cols="12" md="4">
                <b-card class="earnings-card">
                    <b-row>
                        <b-col cols="6">
                            <b-card-title class="mb-1">
                                الاكواد المباعة
                            </b-card-title>
                            <div class="font-small-2">
                                هذا الشهر
                            </div>
                            <h5 class="mb-1">
                                {{homeObj.totalValueCode}}
                            </h5>
                            <b-card-text class="text-muted font-small-2">
                                <span> {{homeObj.lastTotalValueCode ? (100 - Math.floor(homeObj.totalValueCode / homeObj.lastTotalValueCode * 100) >= 0 ? ' ربح اكبر من الشهر الماضي بنسبة' : 'اقل من الشهر بالماضي بنسبة') : 0}} </span>
                                <span class="font-weight-bolder">
                                    {{homeObj.lastTotalValueCode ? (100 - Math.floor(homeObj.totalValueCode / homeObj.lastTotalValueCode * 100)) : 0}}%
                                </span>
                            </b-card-text>
                        </b-col>
                        <b-col cols="6">
                            <vue-apex-charts
                                height="120"
                                :options="earningsChart.chartOptions"
                                :series="[homeObj.totalCodeCount]"
                            />
                        </b-col>
                    </b-row>
                </b-card>
                <b-card class="earnings-card">
                    <b-row>
                        <b-col cols="6">
                            <b-card-title class="mb-1">
                                الاكواد المستحقة
                            </b-card-title>
                            <div class="font-small-2">
                                هذا الشهر
                            </div>
                            <h5 class="mb-1">
                                {{homeObj.worthlyValueCode}}
                            </h5>
                            <b-card-text class="text-muted font-small-2">
                                <span> {{homeObj.lastworthlyValueCode ? (100 - Math.floor(homeObj.worthlyValueCode / homeObj.lastworthlyValueCode * 100) >= 0 ? ' ربح اكبر من الشهر الماضي بنسبة' : 'اقل من الشهر بالماضي بنسبة') : 0}} </span>
                                <span class="font-weight-bolder">
                                    {{homeObj.lastworthlyValueCode ? (100 - Math.floor(homeObj.worthlyValueCode / homeObj.lastworthlyValueCode * 100)) : 0}}%
                                </span>
                            </b-card-text>
                        </b-col>
                        <b-col cols="6">
                            <vue-apex-charts
                                height="120"
                                :options="earningsChart.chartOptions"
                                :series="[homeObj.worthlyCodeCount]"
                            />
                        </b-col>
                    </b-row>
                </b-card>
            </b-col>
        </b-row>
    </div>
</template>
<style>
.apexcharts-tooltip-marker {
    margin-left: 0;
    margin-right: 10px;
}
.stats {
    height: 134px;
    align-items: center;
    place-content: center;
    display: flex;
}
.mutedActivity {
    position: absolute;
    bottom: 0;
}
</style>
<script>
import StatisticCardWithAreaChart from "@core/components/statistics-cards/StatisticCardWithAreaChart.vue";
import VueApexCharts from "vue-apexcharts";
import { $themeColors } from "@themeConfig";
import { BRow, BCol } from "bootstrap-vue";
import StatisticCardHorizontal from "@core/components/statistics-cards/StatisticCardHorizontal.vue";

import { mapActions, mapState } from "vuex";

export default {
    components: {
        VueApexCharts,
        StatisticCardWithAreaChart,
        BRow,
        BCol,

        StatisticCardHorizontal
    },
    computed: {
        ...mapState({
            homeObj: state => state.home.homeObj,
            usersMonthlyCount: state => state.home.usersMonthlyCount,
            sellersMonthlyCount: state => state.home.sellersMonthlyCount,
            invoicesCount: state => state.home.invoicesCount,
            yearEarnning: state => state.home.yearEarnning,
            years: state => state.home.years,
        })
    },
    data() {
        return {
            selectedYear: 0,

            chartOptions: {
                chart: {
                    type: "bar",
                    height: 350
                },
                plotOptions: {
                    bar: {
                        horizontal: false,
                        columnWidth: "55%",
                        endingShape: "rounded"
                    }
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    show: true,
                    width: 2,
                    colors: ["transparent"]
                },
                xaxis: {
                    categories: [
                        "Jan",
                        "Feb",
                        "Mar",
                        "Apr",
                        "May",
                        "Jun",
                        "Jul",
                        "Aug",
                        "Sep",
                        "Oct",
                        "Nov",
                        "Dec"
                    ]
                },
                fill: {
                    opacity: 1
                },
                tooltip: {
                    y: {
                        formatter: function(val) {
                            return "$ " + val + " thousands";
                        }
                    }
                }
            },
            areaLineChart: {
                chart: {
                    toolbar: {
                        show: false
                    },
                    sparkline: {
                        enabled: true
                    }
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: "smooth",
                    width: 2.5
                },
                xaxis: {
                    labels: { show: false },
                    categories: [
                        "Jan",
                        "Feb",
                        "Mar",
                        "Apr",
                        "May",
                        "Jun",
                        "Jul",
                        "Aug",
                        "Sep",
                        "Oct",
                        "Nov",
                        "Dec"
                    ]
                },
                tooltip: {
                    show: false
                } 
            },

            earningsChart: {
                chartOptions: {
                    chart: {
                        type: "donut",
                        toolbar: {
                            show: false
                        }
                    },
                    dataLabels: {
                        enabled: false
                    },
                    legend: { show: false },
                    labels: ["الاكواد"],
                    colors: [
                        $themeColors.primary
                    ],
                    plotOptions: {
                        pie: {
                            donut: {
                                
                                labels: {
                                    show: true,
                                    value: {
                                        offsetY: -10,
                                    },
                                    total: {
                                        show: true,
                                        offsetY: -5,
                                        label: "",
                                    }
                                }
                            }
                        }
                    }
                }
            },

            // budget chart
            budgetChart: {
                options: {
                    chart: {
                        height: 80,
                        toolbar: { show: false },
                        zoom: { enabled: false },
                        type: "line",
                        sparkline: { enabled: true }
                    },
                    stroke: {
                        curve: "smooth",
                        dashArray: [0, 5],
                        width: [2]
                    },
                    colors: [$themeColors.primary, "#dcdae3"],
                    tooltip: {
                        enabled: false
                    }
                }
            }
        };
    },
    created() {
        this.getHome();
    },
    methods: {
        ...mapActions(["getHome"])
    }
};
</script>
