import BaseService from "./BaseService";
import { ControllerRoutes, ProductRoutes } from "@/helpers/RouteConstraints";
import axios from "axios";
import { store } from "@/store";
import AccountService from "./AccountService";

export default class ProductService extends BaseService {
  constructor() {
    super(ControllerRoutes.Product);
  }

  async productAddVersion(productId, versionInfoId, priorityId) {
    return await this.request("GET", ProductRoutes.AddVersion(productId, versionInfoId, priorityId));
  }

  async productRemoveVersion(productId, versionInfoId) {
    return await this.request("DELETE", ProductRoutes.RemoveVersion(productId, versionInfoId));
  }

  async productAddModule(productId, moduleId) {
    return await this.request("GET", ProductRoutes.AddModule(productId, moduleId));
  }

  async productRemoveModule(productId, moduleId) {
    return await this.request("DELETE", ProductRoutes.RemoveModule(productId, moduleId));
  }


  async productExistSetup(platformId, productId, versionInfoId) {
    return await this.request("GET", ProductRoutes.ExistSetup(platformId, productId, versionInfoId));
  }

  //İSTİSNA ONUPLOADPROGRESS KISMI İÇİN ŞUANLIK BİR ÇÖZÜM BULUNMADI
  async productAddSetup(platformId, productId, versionInfoId, formData) {
    return axios({
      method: "post",
      url: ProductRoutes.AddSetup(platformId, productId, versionInfoId),
      headers: this.getRequestHeader(),
      data: formData,
      onUploadProgress(progressEvent) {
        let val = parseInt(Math.round((progressEvent.loaded / progressEvent.total) * 100));
        if (platformId === 1) store.state.progress.uploadProgressL = val;
        if (platformId === 2) store.state.progress.uploadProgressM = val;
        if (platformId === 3) store.state.progress.uploadProgressW = val;
      },
    })
      .then((response) => {
        return response.data;
      })
      .catch(async (err) => {
        if (err.response.status === 401) {
          await new AccountService().loginByRefreshToken();
        }
      });
  }
}
