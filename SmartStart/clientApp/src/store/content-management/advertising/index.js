import api from '@api';
export default {
    state: {
        advertising:[],
        advertisingDto:{
            id: '',
            imageFile: '',
            title: "",
            imagePath: "",
            startDate: "",
            endDate: "",
            type:0,
            videoPath:'',
            videoFile:''
        }
    },
    mutations: {
        Get_Advertising(state, payload) {
            state.advertising = payload
        },
        Add_Advertising(state,payload){
            state.advertising.unshift(payload)
        },
        Update_Advertisment(state, data) {
            Object.assign(state.advertising.find((item) => item.id == data.id), data);
        },
        Reset_Advertising(state){
            Object.assign(
                state.advertisingDto,{
                    id: '',
                    imageFile: '',
                    title: "",
                    imagePath: "",
                    startDate: "",
                    endDate: "",
                    type:0,
                    videoPath:'',
                    videoFile:''
                }
            )
          
        },
        delete_Advertising(state,id){
            state.advertising.splice(state.advertising.findIndex(item => item.id == id), 1);
        }
    },
    actions: {
        getAdvertising({commit}) {
            api.get('Advertisement/Fetch', ({data}) => {
                console.log(data)
                commit('Get_Advertising', data)
            })
        },
        addAdvertising({commit},payload){
            
            for (var value of payload.formData.values()) {
                console.log(value);
            }
            api.post('Advertisement/Upload', payload.formData, ({data}) => {
                if(!payload.id) {
                    commit("Add_Advertising", data);
                } else {
                    console.log(payload.formData)
                    commit("Update_Advertisment", data);
                }
            },
            {
                
                success:!payload.id ?  "تم إضافة الإعلان بنجاح":" تم تعديل الإعلان بنجاح",
                error: !payload.id? "فشل إضافة الإعلان ":"فشل تعديل الإعلان"
               
            }
            )
        },
        deleteAdvertising({commit},id){
            api.delete("Advertisement/Delete?id=" + id, ({ data }) => {
                if(data) {
                    commit("delete_Advertising",id)
                }
            },
            {
                    confirm: "هل انت متأكد من حذف الإعلان",
                    success: "تم حذف الإعلان بنجاح",
                    error: "فشل حذف الإعلان"
                }
           
            );
        }

    }
}