export default class Tree {
    constructor(){}
    
    insertInTree(node, rootNode, emplyeesCounts) {
        if (node.parentId === null) {
            if(rootNode.positionDtos === null) {
                rootNode.positionDtos = [{
                    id: node.id,
                    branchId: node.branchId,
                    positionName: node.positionName,
                    isChild: node.isChild,
                    isParent: node.isParent,
                    parentId: node.parentId,
                    emplyeesCount: emplyeesCounts.get(node.id),
                    isManager: node.isManager,
                    roles: node.roles,
                    positionDtos: node.positionDtos,
                }]
            } else {
                rootNode.positionDtos.push({
                    id: node.id,
                    branchId: node.branchId,
                    positionName: node.positionName,
                    isChild: node.isChild,
                    emplyeesCount: emplyeesCounts.get(node.id),
                    isParent: node.isParent,
                    parentId: node.parentId,

                    isManager: node.isManager,
                    roles: node.roles,
                    positionDtos: node.positionDtos,
                });
            }
        } else {
            const positionDtos = rootNode.positionDtos;
            for (let i = 0; i < positionDtos.length; i++) {
                const currentNode = positionDtos[i];
                if (currentNode.id === node.parentId) {
                    currentNode.positionDtos === null // =>
                    ? currentNode.positionDtos = [{
                        id: node.id,
                        branchId: node.branchId,
                        positionName: node.positionName,
                        isParent: node.isParent,
                        emplyeesCount: emplyeesCounts.get(node.id),
                        isChild: node.isChild,
                        parentId: node.parentId,

                        isManager: node.isManager,
                        roles: node.roles,
                        positionDtos: node.positionDtos,
                    }] // =>
                    : currentNode.positionDtos.push({
                        id: node.id,
                        branchId: node.branchId,
                        positionName: node.positionName,
                        isParent: node.isParent,
                        emplyeesCount: emplyeesCounts.get(node.id),
                        isChild: node.isChild,
                        parentId: node.parentId,

                        isManager: node.isManager,
                        roles: node.roles,
                        positionDtos: node.positionDtos,
                    });
                    return false;
                } else if (currentNode.positionDtos) {
                    this.insertInTree(node, currentNode, emplyeesCounts);
                } 
            }
        }
    }

    updateTree(node, rootNode){
        if (node.parentId === null) {
            rootNode.positionDtos.forEach((item) => {
                if (item.id === node.id){
                    Object.assign(item, node);
                }
            });
        } else {
            const positionDtos = rootNode.positionDtos;
            for (let i = 0; i < positionDtos.length; i++) {
                const currentNode = positionDtos[i];
                if (currentNode.id === node.parentId){
                    currentNode.positionDtos.forEach((item) => {
                        if (item.id === node.id) {
                            Object.assign(item, node);
                        }
                    });
                    return false;
                } else if (currentNode.positionDtos) {
                    this.updateTree(node, currentNode);
                }
            }
        }
    }
    
    printTree(node) {
        const positionDtos = node.positionDtos;
        if (positionDtos) {
            positionDtos.forEach(currentNode => {
                console.log(currentNode);
                this.printTree(currentNode);
            });
        }
    }

    deleteNode(id, node) {
        const positionDtos = node.positionDtos;
        if (positionDtos) {
            for (let i = 0; i < positionDtos.length; i++) {
                if (positionDtos[i].id !== id) {
                    this.deleteNode(id, positionDtos[i]);
                } else {
                    positionDtos.splice(i, 1);
                    return false;
                }
            }
        }
    }


    // un used func
    removeAllChildNodes(id, node) {
        const child = node.child;
        if (child) {
            for (let i = 0; i < child.length; i++) {
                if (child[i].id !== id) {
                    this.removeAllChildNodes(id, child[i]);
                } else {
                    child[i].child = null;
                    return false;
                }
            }
        }
    }
}


