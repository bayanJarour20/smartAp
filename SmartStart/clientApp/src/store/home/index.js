import api from "@api";

export default {
    state: {
        homeObj: {
            subjectCount: 0,
            tagCount: 0,
            examCount: 0,
            interviewCount: 0,
            bankCount: 0,
            microscopeCount: 0,
            totalValueCode: 0,
            totalCodeCount: 0,
            worthlyValueCode: 0,
            worthlyCodeCount: 0,
            lastTotalValueCode: 0,
            lastworthlyValueCode: 0,
            usersMonthly: [],
            sellersMonthly: [],
            invoices: []
        },
        yearEarnning: 0,
        usersMonthlyCount: [],
        sellersMonthlyCount: [],
        invoicesCount: [[], [], [], [], []],
        years: []
    },
    mutations: {
        get_Home(state, data) {
            state.yearEarnning = 0
            Object.assign(state.homeObj, data);
            state.usersMonthlyCount = [];
            state.sellersMonthlyCount = [];
            data.usersMonthly.forEach(countUser => {
                state.usersMonthlyCount.push(countUser.count);
            }),
                data.sellersMonthly.forEach(countUser => {
                    state.sellersMonthlyCount.push(countUser.count);
                }),
                (state.years = []);
            data.invoices.forEach((invoice, index) => {
                state.years.push({
                    id: index,
                    name: invoice.year
                });
                state.invoicesCount[index] = [];
                invoice.data.forEach(countData => {
                    state.yearEarnning += countData.total
                    state.invoicesCount[index].push(countData.total);
                });
            });
        }
    },
    actions: {
        getHome({ commit }) {
            api.get("Home", ({ data }) => {
                commit("get_Home", data);
            });
        }
    }
};
