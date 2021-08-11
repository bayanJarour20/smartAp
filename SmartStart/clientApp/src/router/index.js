import Vue from "vue";
import VueRouter from "vue-router";
import store from "@/store";

// Routes
// import { canNavigate } from "@/libs/acl/routeProtection";
// import { isUserLoggedIn } from "@/auth/utils";

import docs from "./docs";
import contentManagement from "./content-management";

Vue.use(VueRouter);

export const All = "all";
export const Admin = "Admin";

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    scrollBehavior() {
        store.dispatch("app/setLoading", false);
        return { x: 0, y: 0 };
    },
    routes: [
        {
            path: "/home",
            name: "home",
            component: () => import("@/views/home/index.vue"),
            meta: () => ({
                pageTitle: "الرئيسية",
                icon: "",
                roles: [All],
                breadcrumb: [
                    {
                        text: "الرئيسية",
                        active: true,
                    },
                ],
            }),
        },
        ...contentManagement,

        // test components
        {
            path: "/settings",
            name: "settings",
            component: () => import("@/views/settings"),
            meta: () => ({
                pageTitle: "الإعدادات",
                icon: "",
                roles: [Admin],
                breadcrumb: [
                    {
                        text: "الإعدادات",
                        active: true,
                    },
                ],
            })
        },
        ...docs,
        {
            path: "/login",
            name: "login",
            component: () => import("@/views/Login.vue"),
            meta: () => ({
                pageTitle: "Smart Start تسجيل الدخول",
                layout: "full",
                roles: [All],
                redirectIfLoggedIn: true,
            }),
        },
        {
            path: "/access-denied",
            name: "access-denied",
            component: () => import("@/views/error/NotAuthorized.vue"),
            meta: () => ({
                pageTitle: "ممنوع الوصول",
                layout: "full",
                roles: [All],
            }),
        },
        {
            path: "/server-error",
            name: "server error",
            component: () => import("@/views/error/server-error.vue"),
            meta: () => ({
                pageTitle: "خطأ في الإتصال",
                layout: "full",
                roles: [All],
            }),
        },
        {
            path: "/error-404",
            name: "error 404",
            meta: () => ({
                pageTitle: "الصفحة غير موجودة",
                layout: "full",
                roles: [All],
            }),
            component: () => import("@/views/error/error-404.vue"),
        },
        {
            path: "/",
            meta: () => ({
                pageTitle: "الرئيسية",
            }),
            redirect: "/home",
        },
        {
            path: "*",
            meta: () => ({
                pageTitle: "الصفحة غير موجودة",
            }),
            redirect: "error-404",
        },
    ],
});

router.beforeEach((to, from, next) => {
    store.commit('app/Update_Prev_Link', from)
    // const isLoggedIn = isUserLoggedIn();
    document.title = to.meta(to).pageTitle

    // if (!canNavigate(to)) {
    //     // Redirect to login if not logged in
    //     if (!isLoggedIn) return next("/login");

    //     // If logged in => not authorized
    //     return next("/access-denied");
    // }
    // Redirect if logged in
    // if (to.meta(to).redirectIfLoggedIn && isLoggedIn) {
    //     const userData = getUserData();
    //     next(getHomeRouteForLoggedInUser(userData ? userData.role : null));
    // }

    return next();
});

export default router;
