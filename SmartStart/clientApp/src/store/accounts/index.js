import api from "@api";
import router from "@/router";
export default {
    state: {
        rolesList:[],
        usersList: [],
        posDto: {
            id: "",
            name: "",
            userName: "",
            email: "",
            password: "",
            phoneNumber: "",
            birthday: new Date(1970),
            codeDetailsSimpleDto: [],
            facIds: [],
            dateBlocked: null,
            address: "",
            gender: false,
            moneyLimit: 0,
            subscriptionDate: new Date(),
            codeSoldCount: 0,
            allowDiscount: false,
            cityId: "",
            dateActivated: new Date(),
            rate: 0
        },
        accountDto: {
            id: "",
            name: "",
            userName: "",
            email: "",
            password: "",
            phoneNumber: "",
            birthday: new Date(1970),
            address: "",
            subscriptionDate: new Date(),
            dateBlocked: null,
            dateActivated: new Date(),
            gender: false,
            facultyId: null,
            role:0,
        },
        userDto: {
            id: "",
            name: "",
            userName: "",
            codes: [],
            email: "",
            password: "",
            phoneNumber: "",
            birthday: new Date(1970),
            address: "",
            subscriptionDate: new Date(),
            dateBlocked: null,
            dateActivated: new Date(),
            gender: false,
            facultyId: null,
            faculties: []
        }
    },
    mutations: {
        Get_Roles(state, payload) {
            state.rolesList = payload;
        },
        Get_Users(state, payload) {
            state.usersList = payload;
        },
        Create_User(state, payload) {
            state.usersList.unshift(payload);
        },

        Account_Details(state, payload) {
            Object.assign(state.accountDto, payload);
        },
        POS_Details(state, payload) {
            Object.assign(state.posDto, payload);
        },
        User_Details(state, payload) {
            Object.assign(state.userDto, payload);
        },

        Reset_Account_Dto(state) {
            Object.assign(state.accountDto, {
                id: "",
                name: "",
                userName: "",
                email: "",
                password: "",
                phoneNumber: "",
                birthday: new Date(1970),
                address: "",
                subscriptionDate: new Date(),
                dateBlocked: null,
                dateActivated: new Date(),
                gender: false,
                facultyId: null,
                role:0
            });
        },
        Reset_Pos_Dto(state) {
            Object.assign(state.posDto, {
                id: "",
                name: "",
                userName: "",
                email: "",
                password: "",
                phoneNumber: "",
                birthday: new Date(1970),
                codeDetailsSimpleDto: [],
                facIds: [],
                dateBlocked: null,
                address: "",
                gender: false,
                moneyLimit: 0,
                subscriptionDate: new Date(),
                codeSoldCount: 0,
                allowDiscount: false,
                cityId: "",
                dateActivated: new Date(),
                rate: 0
            });
        },
        Reset_User_Dto(state) {
            Object.assign(state.userDto, {
                id: "",
                name: "",
                userName: "",
                codes: [],
                email: "",
                password: "",
                phoneNumber: "",
                birthday: new Date(1970),
                address: "",
                subscriptionDate: new Date(),
                dateBlocked: null,
                dateActivated: new Date(),
                gender: false,
                facultyId: null,
            });
        },

        Block_Pos(state) {
            state.posDto.dateBlocked = state.posDto.dateBlocked
                ? null
                : new Date();
        },
        Block_User(state) {
            state.userDto.dateBlocked = state.userDto.dateBlocked
                ? null
                : new Date();
        },
        Block_Account(state) {
            state.accountDto.dateBlocked = state.accountDto.dateBlocked
                ? null
                : new Date();
        },
        Delete_Users_List(state,payload){
            let MapOfIds = new Map(); 
                var idx; 
                var tempList = []; 
                for(idx = 0 ; idx < payload.length ; idx++) {
                    MapOfIds.set(payload[idx] , 1);
                }
                for(idx = 0 ; idx < state.usersList.length ; idx++) {
                    if(MapOfIds.has(state.usersList[idx].id) === false) {
                        tempList.push(state.usersList[idx]); 
                    }
                }
                state.usersList = tempList; 
        },
        User_List_Dto(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.userDto.codes.length ; idx++) {
                if(MapOfIds.has(state.userDto.codes[idx].id) === false) {
                    tempList.push(state.userDto.codes[idx]); 
                }
            }
            state.userDto.codes = tempList; 
         
        },
        Code_Point_List_Dto(state,payload){
            let MapOfIds = new Map(); 
                var idx; 
                var tempList = []; 
                for(idx = 0 ; idx < payload.length ; idx++) {
                    MapOfIds.set(payload[idx] , 1);
                }
                for(idx = 0 ; idx < state.posDto.codeDetailsSimpleDto.length ; idx++) {
                    if(MapOfIds.has(state.posDto.codeDetailsSimpleDto[idx].id) === false) {
                        tempList.push(state.posDto.codeDetailsSimpleDto[idx]); 
                    }
                }
                state.posDto.codeDetailsSimpleDto = tempList; 
        }
        
     
    },
    actions: {
        getRoles({ commit }){
            api.get("Account/Roles", ({ data }) => {
                commit("Get_Roles", data);
            });
        },
        getUsers({ commit }, type) {
            if (type == 1) {
                api.get("PointOfSale/GetAll", ({ data }) => {
                    commit("Get_Users", data);
                });
            } else if (type == 2) {
                api.get("Account/GetAllDashboard", ({ data }) => {
                    commit("Get_Users", data);
                });
            } else if (type == 0) {
                api.get("User/GetAllUser", ({ data }) => {
                    commit("Get_Users", data);
                });
            }
        },
        // pos
        createPOS({ commit }, payload) {
            api.post(
                "PointOfSale/Create",
                {
                    address: payload.address,
                    birthday: payload.birthday,
                    dateBlocked: null,
                    email: payload.email,
                    name: payload.name,
                    password: payload.password,
                    phoneNumber: payload.phoneNumber,
                    subscriptionDate: new Date(payload.subscriptionDate),
                    userName: payload.userName,
                    gender: payload.gender,
                    moneyLimit: payload.moneyLimit,
                    codeSoldCount: payload.codeSoldCount,
                    allowDiscount: payload.allowDiscount,
                    cityId: payload.cityId,
                    dateActivated: new Date(payload.dateActivated),
                    rate: (payload.rate / 100),
                    facList: payload.facIds
                },
                ({ data }) => {
                    commit("Create_User", data);
                },{
                    success:"تم إضافة نقطة البيع بنجاح",
                    error:"فشل إضافة نقطة البيع"
                }
            );
        },
        blockPOS({ commit }, id) {
            api.put("PointOfSale/Block/" + id, {}, () => {
                commit("Block_Pos");
            },
            {
                success:"تمت العملية بنجاح",
                error:"فشلت العملية"
            }
            );
        },
        deletePOS(ctx, id) {
            api.delete("PointOfSale/Delete?id=" + id, ({ data }) => {
                if (data) {
                    router.push("/users/1");
                }
            },
            {
                confirm:"هل أنت متأكد من حذف نقاط البيع",
                success:"تم تعديل نقاط البيع بنجاح",
                error:"فشل تعديل نقاط البيع"
            });
        },
        updatePOS(ctx, payload) {
            api.put("PointOfSale/Update", {
                id: payload.id,
                address: payload.address,
                birthday: payload.birthday,
                dateBlocked: new Date(payload.dateBlocked),
                email: payload.email,
                name: payload.name,
                password: payload.password,
                phoneNumber: payload.phoneNumber,
                subscriptionDate: new Date(payload.subscriptionDate),
                userName: payload.userName,
                gender: payload.gender,
                moneyLimit: payload.moneyLimit,
                codeSoldCount: payload.codeSoldCount,
                allowDiscount: payload.allowDiscount,
                cityId: payload.cityId,
                dateActivated: new Date(payload.dateActivated),
                rate: (payload.rate / 100),
                facList : payload.facList
            },
            () => {},
            {
                success:"تم تعديل نقاط البيع بنجاح",
                error:"فشل تعديل نقاط البيع"
            });
        },
        posDetails({ commit }, id) {
            api.get("PointOfSale/Details/" + id, ({ data }) => {
                commit("POS_Details", data);
            });
        },
        PosDetailsDto({commit},ids){
            api.delete("PointOfSale/MultiDelete",({ data }) => {
                if(data) {
                    commit("Pos_Details_Dto", ids);
                }
            },{confirm: 'هل تريد فعلا حذف نقاط البيع المحددة', success: 'تم حذف نقاط البيع المحددة بنجاح', error: "فشل حذف نقاط البيع المحددة " },
            ids)
        },

        // account
        createAccount({ commit }, payload) {
            api.post(
                "Account/Create",
                {
                    userName: payload.userName,
                    password: payload.password,
                    email: payload.email,
                    name: payload.name,
                    address: payload.address,
                    phoneNumber: payload.phoneNumber,
                    birthday: new Date(payload.birthday),
                    dateBlocked: null,
                    dateActivated: new Date(payload.dateActivated),
                    subscriptionDate: new Date(payload.subscriptionDate),
                    gender: payload.gender,
                    facultyId: payload.facultyId,
                    role: payload.role,
                },
                ({ data }) => {
                    commit("Create_User", data);
                },{
                    success:"تم إضافة المستخدم بنجاح",
                    error:"فشل إضافة المستخدم "
                }
            );
        },
        accountDetails({ commit }, id) {
            api.get("Account/Details/" + id, ({ data }) => {
                commit("Account_Details", data);
            });
        },
        blockAccount({ commit }, id) {
            api.put("Account/Block/" + id, {}, () => {
                commit("Block_Account");
            }
            ,
            {
                success:"تمت العملية بنجاح",
                error:"فشلت العملية"
            });
        },
        deleteAccount(ctx, id) {
            api.delete("Account/Delete?id=" + id, ({ data }) => {
                if (data) {
                    router.push("/users/2");
                }
            },
            {
                confirm:"هل أنت متأكد من حذف المستخدم",
                success:"تم تعديل المستخدم بنجاح",
                error:"فشل تعديل  المستخدم"
            });
        },
        updateAccount(ctx, payload) {
            api.put("Account/Update", {
                id: payload.id,
                userName: payload.userName,
                password: payload.password,
                email: payload.email,
                name: payload.name,
                address: payload.address,
                phoneNumber: payload.phoneNumber,
                birthday: new Date(payload.birthday),
                dateBlocked: null,
                dateActivated: new Date(payload.dateActivated),
                subscriptionDate: new Date(payload.subscriptionDate),
                gender: payload.gender,
                facultyId: payload.facultyId,
                role: payload.role,
            }
            , () => {}
            ,
            {
                
                success: "تم تعديل المستخدم بنجاح",
                error: "فشل تعديل المستخدم"
            });
        },

        // user
        createUser({ commit }, payload) {
            api.post(
                "User/Create",
                {
                    userName: payload.userName,
                    password: payload.password,
                    email: payload.email,
                    name: payload.name,
                    address: payload.address,
                    phoneNumber: payload.phoneNumber,
                    birthday: new Date(payload.birthday),
                    dateBlocked: null,
                    dateActivated: new Date(payload.dateActivated),
                    subscriptionDate: new Date(payload.subscriptionDate),
                    gender: payload.gender,
                    facultyId: payload.facultyId
                },
                ({ data }) => {
                    commit("Create_User", data);
                }
                ,{
                    success: "تم إضافة المستخدم بنجاح",
                    error: "فشل  إضافة المستخدم"
                    }
            );
        },
        userDetails({ commit }, id) {
            api.get("User/Details/" + id, ({ data }) => {
                commit("User_Details", data);
            });
        },
        blockUser({ commit }, id) {
            api.put("User/Block/" + id, {}, () => {
                commit("Block_User");
            },
            {
                
                success: "تمت العملية بنجاح",
                error: " فشلت العملية"
            });
        },
        updateUser(ctx, payload) {
            api.put("User/Update", {
                id: payload.id,
                userName: payload.userName,
                password: payload.password,
                email: payload.email,
                name: payload.name,
                address: payload.address,
                phoneNumber: payload.phoneNumber,
                birthday: new Date(payload.birthday),
                dateBlocked: null,
                dateActivated: new Date(payload.dateActivated),
                subscriptionDate: new Date(payload.subscriptionDate),
                gender: payload.gender,
                facultyId: payload.facultyId
            }, () => {}
            ,
            {
                
                success: "تم تعديل المستخدم بنجاح",
                error: "فشل تعديل المستخدم"
            });
        },
        deleteUser(ctx, id) {
            api.delete("User/Delete?id=" + id, ({ data }) => {
                if (data) {
                    router.push("/users/0");
                }
            },
            {
                confirm: "هل انت متأكد من حذف المستخدم",
                success: "تم حذف المستخدم بنجاح",
                error: "فشل حذف المستخدم"
            });
        },
        deleteUsersList({ commit }, ids){
            console.log(ids)
            api.delete("User/DeleteRange",({ data }) => {
                if(data) {
                    commit("Delete_Users_List", ids);
                }
            },{confirm: 'هل تريد فعلا حذف المستخدمين المحددين', success: 'تم حذف المستخدمين المحددين بنجاح', error: "فشل حذف المستخدمين المحددين " },
            ids)
        },
        UserListDto({commit},ids){
            api.delete("Code/RemoveCodes",({ data }) => {
                if(data) {
                    commit("User_List_Dto", ids);
                }
            },{confirm: 'هل تريد فعلا حذف الإشتركات المحددة', success: 'تم حذف الإشتركات المحددة بنجاح', error: "فشل حذف الإشتركات المحددة " },
            ids)
        },
        CodePointListDto({commit},ids){
            api.delete("Code/RemoveCodes",({ data }) => {
                if(data) {
                    commit("Code_Point_List_Dto", ids);
                }
            },{confirm: 'هل تريد فعلا حذف الأكواد المحددة', success: 'تم حذف الأكواد المحددة بنجاح', error: "فشل حذف الأكواد المحددة " },
            ids)
        }


    }
};
