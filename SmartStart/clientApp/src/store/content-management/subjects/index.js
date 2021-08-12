import api from "@api";
import router from "@/router";
import store from "@/store";
export default {
    state: {
        subjectsList: [],
        subjectDto: {
            id: '',
            name: '',
            type: 0,
            imagePath: '',
            doctors:[],
            description: '',
            isFree:true
        }
    },
    mutations: {
        Fetch_Subject(state, payload) {
            state.subjectsList = payload
        },
        Upload_Subject_Create(state, payload) {
            state.subjectsList.unshift(payload)
        },
        Reset_Subject_Dto(state) {
            Object.assign(
                state.subjectDto, {
                    id: '',
                    name: '',
                    type: 0,
                    imagePath: '',
                    doctors:[],
                    description: '',
                    isFree:true
                })
        },
        Subject_Details(state, payload) {
            Object.assign(state.subjectDto, {
                id: payload.id,
                name: payload.name,
               
            
                type: payload.type,
                imagePath: payload.imagePath,
               
               
                doctorsId: payload.tagIds.filter((id) => {
                    return store.getters.doctors.findIndex(i => i.id == id) != -1
                }),
                semesterId: payload.semesterId,
                sectionId: payload.sectionId,
                description: payload.description
            })
        }
    },
    actions: {
        fetchSubject({commit}) {
            api.get('Subject/GetAll', ({data}) => {
                commit('Fetch_Subject', data)
            })
        },
        uploadSubject({commit}, payload) {
            api.post('Subject/Upload', payload.formData, ({data}) => {
                if(!payload.id) {
                    commit('Upload_Subject_Create', data)
                }
            })
        },
        addSubject({commit},payload){
            
           
                api.post('Subject/SetSubject', payload.formData, ({data}) => {
                    if(!payload.id) {
                        commit('Upload_Subject_Create', data)
                    }
                })
            
        },
        subjectDetails({commit}, id) {
            api.get('Subject/Details/' + id, ({data}) => {
                commit('Subject_Details', data)
            })
        },
        deleteSubject(ctx, id) {
            api.delete('Subject/Delete/' + id, ( ) => {
               
                    router.push('/subjects')
                 
            })
        }

    }
}