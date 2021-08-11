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
                }
            );
        },
        blockPOS({ commit }, id) {
            api.put("PointOfSale/Block/" + id, {}, () => {
                commit("Block_Pos");
            });
        },
        deletePOS(ctx, id) {
            api.delete("PointOfSale/Delete?id=" + id, ({ data }) => {
                if (data) {
                    router.push("/users/1");
                }
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
            });
        },
        posDetails({ commit }, id) {
            api.get("PointOfSale/Details/" + id, ({ data }) => {
                commit("POS_Details", data);
            });
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
            });
        },
        deleteAccount(ctx, id) {
            api.delete("Account/Delete?id=" + id, ({ data }) => {
                if (data) {
                    router.push("/users/2");
                }
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
            });
        },
        deleteUser(ctx, id) {
            api.delete("User/Delete?id=" + id, ({ data }) => {
                if (data) {
                    router.push("/users/0");
                }
            });
        },


    }
};
