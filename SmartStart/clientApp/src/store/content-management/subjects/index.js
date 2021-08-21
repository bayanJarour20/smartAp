import api from "@api";
 import router from "@/router";
// import store from "@/store";
export default {
    state: {
        subjectsList: [],
        subjectDto: {
            id: '00000000-0000-0000-0000-000000000000',
            name: '',
            isFree:false,
            file: null,
            type: 0,
            imagePath: '',
            subjectFaculties:[],
            description: ''
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
                    id: '00000000-0000-0000-0000-000000000000',
                    name: '',
                    isFree:false,
                    file: null,
                    type: 0,
                    imagePath: '',
                    subjectFaculties:[],
                    description: ''
                })
        },
        Subject_Details(state, payload) {
            Object.assign(state.subjectDto, {
                id: payload.id,
                name: payload.name,
                file: payload.file,
                type: payload.type,
                imagePath: payload.imagePath,
                description: payload.description
            })
        },
        subj_List_Dto(state,payload){
            
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.subjectsList.length ; idx++) {
                if(MapOfIds.has(state.subjectsList[idx].id) === false) {
                    tempList.push(state.subjectsList[idx]); 
                }
            }
            state.subjectsList = tempList; 
        },
        delete_Subject(state,id){
            state.subjectsList.splice(
                state.subjectsList.findIndex(item => item.id == id), 1)
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
        subjectDetails({commit}, id) {
            api.get('Subject/Details/' + id, ({data}) => {
                commit('Subject_Details', data)
            })
        },
        deleteSubject({commit}, id) {    
            console.log(id)  
            api.delete("Subject/RemoveSubject?subjectId=" + id, ({ data }) => {
                if(data) {
                    router.push('/subjects')
                    commit("delete_Subject", id);
                }
            },{confirm: 'هل تريد فعلاً حذف المادة', success: 'تم حذف المادة بنجاح', error: "فشل حذف المادة" })
                 
            
        },
        subjListDto({commit},ids){    
            api.delete("Subject/RemoveSubjects",({ data }) => {
                if(data) {
                    commit("subj_List_Dto", ids);
                }
            },{confirm: 'هل تريد فعلا حذف المواد المحددة', success: 'تم حذف المواد المحددة بنجاح', error: "فشل حذف المواد المحددة " },
            ids)
        }

    }
}