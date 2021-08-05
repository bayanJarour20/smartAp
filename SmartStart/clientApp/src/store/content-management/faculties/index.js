import api from "@api";
export default {
    state: {
        faculties: [],
        facultyDto: {
            id: '',
            name: '',
            numberOfYear: 0,
            file: null,
            imagePath: '',
            universityId: 0
        },
        facultyFilterDto: {
            semesterId: 0,

        }
    },

    mutations: {
        Get_Faculties_Details(state, payload) {
            state.faculties = payload;
        },
        Create_Faculty(state, data) {
            state.faculties.unshift(data)
        },
        Update_Faculty(state, data) {
            Object.assign(state.faculties.find(item => item.id == data.id), data)
        },
        Set_Facultie_Dto(state, payload) {
            console.log(payload)
            if (payload) {
                Object.assign(
                    state.facultyDto,
                    payload
                )
            } else {
                Object.assign(
                    state.facultyDto, {
                        id: '',
                        name: '',
                        numberOfYear: 0,
                        file: null,
                        imagePath: '',
                        universityId: 0
                    }
                )
            }
        },
        Delete_Faculty(state, id){ 
            state.faculties.splice(
                state.faculties.findIndex(item => item.id == id), 1)
        }
    },
    actions: {
        getFacultiesDetails({ commit }) {
            api.get("Faculty/GetAll", ({ data }) => {
                commit("Get_Faculties_Details", data);
            });
        },
        actionFaculty({commit}, payload) {
            api.post("Faculty/SetFaculty", payload.formData, ({ data }) => {
                if(!payload.id) {
                    commit("Create_Faculty", data);
                } else {
                    commit("Update_Faculty", data);
                }
            })
        },
        deleteFaculty({commit}, id) {
            api.delete("Faculty/RemoveFaculty?id=" + id, ({ data }) => {
                if(data) {
                    commit("Delete_Faculty", id);
                }
            })
        }
    }
};
