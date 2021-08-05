import { All } from "@/router";
import { getUserRoles } from "@/auth/utils";

export const canNavigate = 
    to => {
        const userData = getUserRoles()
        return to.meta(to).roles.indexOf(All) != -1 || (userData && to.meta(to).roles.indexOf(userData) != -1)
    }

export const _ = undefined
