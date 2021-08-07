import api from "@api";
export default {
    state: {
        faculties: [],
        facultyDto: {
            id: '',
            name: '',
            numOfYears: 0,
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
                        numOfYears: 0,
                        file: null,
                        imagePath: '',
                        universityId: 0
                    }
                ),
                console.log(state.facultyDto)

            }
        },
        Delete_Faculty(state, id){ 
            state.faculties.splice(
                state.faculties.findIndex(item => item.id == id), 1)
        },
        Delete_Faculty_List(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.faculties.length ; idx++) {
                if(MapOfIds.has(state.faculties[idx].id) === false) {
                    tempList.push(state.faculties[idx]); 
                }
            }
            state.faculties = tempList; 
         
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
            },
            !payload.id ?{ success: 'تم إضافة الكلية  بنجاح', error: "فشل الإضافة الكلية "}:{success: 'تم التعديل في الكلية  بنجاح', error: "فشل التعديل في الكلية " })
        },
        deleteFaculty({commit}, id) {
            
            api.delete("Faculty/RemoveFaculty?facultyId=" + id, ({ data }) => {
                if(data) {
                    commit("Delete_Faculty", id);
                }
            },{confirm: 'هل تريد فعلاً حذف الكلية', success: 'تم حذف الكلية بنجاح', error: "فشل حذف الكلية" })
        },
        deleteFacultyList({commit}, id) {
            api.put("Faculty/RemoveFaculties",id,({ data }) => {
                if(data) {
                    commit("Delete_Faculty_List", id);
                }
            },{confirm: 'هل تريد فعلاً حذف الكليات المحددة', success: 'تم حذف الكليات المحددة بنجاح', error: "فشل حذف الكليات المحددة " })
        }
    }
};
