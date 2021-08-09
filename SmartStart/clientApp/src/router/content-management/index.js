import { All } from "@/router";
// import store from "@/store";
export default [
    // --- faculties
    {
        path: "/faculties",
        name: "faculties",
        components: {
            default: () => import("@/views/content-management/faculties"),
            'bread-actions' : () => import("@/views/content-management/faculties/components/create-facultie.vue"),
        },
        meta: () => ({
            pageTitle: "الكليات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الكليات",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/advertising",
        name: "advertising",
        components: {
            default: () => import("@/views/content-management/advertising"),
            'bread-actions' : () => import("@/views/content-management/advertising/components/button-create-advertising.vue"),
        },
        meta: () => ({
            pageTitle: "الإعلانات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الإعلانات",
                    active: true,
                },
            ]
        }),
    },
    
]