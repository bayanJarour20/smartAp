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
]