import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { AngularMaterialModule } from "../common/modules-external/angular-material.module";
import { ProductCategoriesTableViewComponent } from "./product-categories/product-categories-table-view/product-categories-table-view.component";
import { ProductCategoryEditorComponent } from "./product-categories/product-category-editor/product-category-editor.component";
import { ProductGroupEditorComponent } from "./product-groups/product-group-editor/product-group-editor.component";
import { ProductGroupsTableViewComponent } from "./product-groups/product-groups-table-view/product-groups-table-view.component";
import { ProductSizeEditorComponent } from "./product-sizes/product-size-editor/product-size-editor.component";
import { ProductSizesTableViewComponent } from "./product-sizes/product-sizes-table-view/product-sizes-table-view.component";
import { ProductsSectionRoutingModule } from "./products-section-routing.module";
import { ProductEditorComponent } from "./products/product-editor/product-editor.component";
import { ProductsTableViewComponent } from "./products/products-table-view/products-table-view.component";

@NgModule({
    imports: [
        FontAwesomeModule,
        HttpClientModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        AngularMaterialModule,
        ProductsSectionRoutingModule
    ],
    declarations: [
        ProductCategoriesTableViewComponent,
        ProductCategoryEditorComponent,
        ProductGroupsTableViewComponent,
        ProductGroupEditorComponent,
        ProductsTableViewComponent,
        ProductEditorComponent,
        ProductSizesTableViewComponent,
        ProductSizeEditorComponent
    ]
})
export class ProductsSectionModule { }
