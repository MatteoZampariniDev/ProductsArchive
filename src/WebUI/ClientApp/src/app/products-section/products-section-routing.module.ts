import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthorizeGuard } from "../../api-authorization/authorize.guard";
import { ProductCategoriesTableViewComponent } from "./product-categories/product-categories-table-view/product-categories-table-view.component";
import { ProductCategoryEditorComponent } from "./product-categories/product-category-editor/product-category-editor.component";
import { ProductGroupEditorComponent } from "./product-groups/product-group-editor/product-group-editor.component";
import { ProductGroupsTableViewComponent } from "./product-groups/product-groups-table-view/product-groups-table-view.component";
import { ProductSizeEditorComponent } from "./product-sizes/product-size-editor/product-size-editor.component";
import { ProductSizesTableViewComponent } from "./product-sizes/product-sizes-table-view/product-sizes-table-view.component";
import { ProductEditorComponent } from "./products/product-editor/product-editor.component";
import { ProductsTableViewComponent } from "./products/products-table-view/products-table-view.component";

export const routes: Routes = [
  { path: 'products/product-categories-table-view', component: ProductCategoriesTableViewComponent },
  { path: 'products/product-category-editor', component: ProductCategoryEditorComponent, canActivate: [AuthorizeGuard] },
  { path: 'products/product-category-editor/:id', component: ProductCategoryEditorComponent, canActivate: [AuthorizeGuard] },

  { path: 'products/product-groups-table-view', component: ProductGroupsTableViewComponent },
  { path: 'products/product-group-editor', component: ProductGroupEditorComponent, canActivate: [AuthorizeGuard] },
  { path: 'products/product-group-editor/:id', component: ProductGroupEditorComponent, canActivate: [AuthorizeGuard] },

  { path: 'products/products-table-view', component: ProductsTableViewComponent },
  { path: 'products/product-editor', component: ProductEditorComponent, canActivate: [AuthorizeGuard] },
  { path: 'products/product-editor/:id', component: ProductEditorComponent, canActivate: [AuthorizeGuard] },

  { path: 'products/product-sizes-table-view', component: ProductSizesTableViewComponent },
  { path: 'products/product-size-editor', component: ProductSizeEditorComponent, canActivate: [AuthorizeGuard] },
  { path: 'products/product-size-editor/:id', component: ProductSizeEditorComponent, canActivate: [AuthorizeGuard] }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProductsSectionRoutingModule { }
