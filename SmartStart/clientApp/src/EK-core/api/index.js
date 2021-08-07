import axiosIns from "@axios";
import { toast } from "@/libs/vue-toast.js";
import Swal from "sweetalert2";

export default {
    post: (
        path,
        body,
        callback = () => {},
        message = { success: "success", error: "error" },
        config = {}
    ) => {
        axiosIns
            .post(path, body, config)
            .then(data => {
                if (data.status == 200) {
                    toast.open({
                        message: message.success,
                        type: "success",
                        duration: 2000,
                        dismissible: true
                    });
                }
                callback(data);
            })
            .catch(() => {
                toast.open({
                    message: message.error,
                    type: "error",
                    duration: 2000,
                    dismissible: true
                });
            });
    },
    put: (
        path,
        body,
        callback = () => {},
        message = { success: "success", error: "error" },
        config = {}
        ) => {
        axiosIns
            .put(path, body, config)
            .then(data => {
                if (data.status == 200) {
                    toast.open({
                        message: message.success,
                        type: "success",
                        duration: 2000,
                        dismissible: true
                    });
                }
                callback(data);
            })
            .catch(() => {
                toast.open({
                    message: message.error,
                    type: "error",
                    duration: 2000,
                    dismissible: true
                });
            });
    },
    get: (path, callback = () => {}) => {
        axiosIns
            .get(path)
            .then(data => {
                callback(data);
            })
            .catch(() => {
                console.log("catch");
            });
    },
    delete: (
        path,
        callback = () => {},
        message = {
            confirm: "you will delete this item",
            success: "success",
            error: "error"
        },
        body = {},
        header = {}
    ) => {
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
                cancelButton: "btn btn-danger"
            },
            buttonsStyling: true
        });
        swalWithBootstrapButtons
            .fire({
                title: "تنبيه!",
                text: message.confirm,
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "نعم",
                cancelButtonText: "إلغاء",
                reverseButtons: false
            })
            .then(result => {
                if (result.value) {
                    axiosIns
                        .delete(path, {header, data: body})
                        .then(data => {
                            if (data.status == 200) {
                                toast.open({
                                    message: message.success,
                                    type: "success",
                                    duration: 2000,
                                    dismissible: true
                                });
                            }
                            callback(data);
                        })
                        .catch(() => {
                            toast.open({
                                message: message.error,
                                type: "error",
                                duration: 2000,
                                dismissible: true
                            });
                        });
                }
            });
    }
};
