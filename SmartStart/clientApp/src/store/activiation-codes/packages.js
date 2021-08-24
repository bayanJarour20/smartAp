import api from "@api";
import router from "@/router";
import store from "@/store";

export default {
    state: {
        packagesList: [],
        packageDto: {
            id: "",
            name: "",
            description: "",
            price: 0,
            type: 0,
            startDate: new Date(),
            endDate: new Date(),
            isHidden: false,
            exams: [],
            selectedExams: []
        },
        tabsFaculty: [],
        filterDto: {
            year: 0,
            semester: 0,
            faculity: 0
        }
    },
    getters: {
        packagesAvaliableList(state) {
            return state.packagesList.filter(
                pack =>
                    new Date(pack.endDate) >= new Date() &&
                    new Date(pack.startDate) <= new Date() && 
                    !pack.isHidden
            );
        }
    },
    mutations: {
        Get_All_Packages(state, payload) {
            state.packagesList = payload;
        },
        Fetch_Tabs_Faculty_By(state, payload) {
            state.tabsFaculty = payload;
        },
        Reset_Package_Dto(state) {
            Object.assign(state.packageDto, {
                id: "",
                name: "",
                description: "",
                price: 0,
                type: 0,
                startDate: new Date(),
                endDate: new Date(),
                isHidden: false,
                exams: [],
                selectedExams: []
            });
        },
        Package_Details(state, data) {
            Object.assign(state.filterDto, {
                year: data.filter.year,
                semester: data.filter.semesteId,
                faculity: data.filter.facultyId
            })
            Object.assign(state.packageDto, {
                id: data.id,
                name: data.name,
                description: data.description,
                price: data.price,
                type: data.type,
                startDate: data.startDate,
                endDate: data.endDate,
                isHidden: data.isHidden,
                exams: [],
                selectedExams: []
            })
            data.exams.forEach(exam => {
                state.tabsFaculty.forEach(sub => {
                    let item = sub.exams.find(item => item.id == exam.examId)
                    if(item) {
                        item.price = exam.price
                    }
                })
                state.packageDto.selectedExams.push(exam.examId)
            });
        }
    },
    actions: {
        getAllPackages({ commit }) {
            api.get("Package/GetAll", ({ data }) => {
                commit("Get_All_Packages", data);
            });
        },
        addPackage({commit}, payload) {
            api.post("Package/Add", payload, () => {
                commit('Reset_Package_Dto')
                router.push("/packages");
            });
        },
        setPackage({commit}, payload) {
            api.put("Package/Update", payload, () => {
                commit('Reset_Package_Dto')
                router.push("/packages");
            });
        },
        packageDetails(ctx, payload) {
            api.get("Package/Details?id=" + payload, ({ data }) => {
                console.log(data)
                store.dispatch('fetchTabsFacultyBy', {
                    year: data.filter.year,
                    semester: data.filter.semesteId,
                    faculity: data.filter.facultyId,
                    data: data
                })
            });
        },
        async fetchTabsFacultyBy({ commit }, payload) {
            return await api.get("Subject/FetchTabsFacultyBy?facultyId=" +
                    payload.faculity +
                    "&year=" +
                    payload.year +
                    "&semesterId=" +
                    payload.semester,
                ({ data }) => {
                    commit("Fetch_Tabs_Faculty_By", data);
                    if(payload.data) {
                        commit("Package_Details", payload.data);
                    }
                }
            );
        }
    }
};
