import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EditorBaseComponent } from "../../../common/base-components/editor-base-component";
import { ProductsSectionRoutes } from "../../../common/enums/products-section-routes";
import { ProductCategoryService } from "../../../common/services/products-section/product-category.service";
import { CreateProductCategoryCommand, UpdateProductCategoryCommand } from "../../../common/services/web-api-client";

@Component({
  selector: 'app-product-category-editor',
  templateUrl: './product-category-editor.component.html'
})
export class ProductCategoryEditorComponent
  extends EditorBaseComponent implements OnInit {
  constructor(protected formBuilder: FormBuilder,
    protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected productCategoryService: ProductCategoryService) {
    super(formBuilder, activatedRoute, router);
  }

  //#region Internal
  protected formName(): string { return this.form.get("name").value; }

  ngOnInit(): void {
    this.getRouteParams();
    this.initializeComponent(
      this.id ? 'Modifica Categoria' : 'Aggiungi Categoria',
      this.id ? 'Modifica una categoria esistente.' : 'Crea una nuova categoria.',
      ProductsSectionRoutes.ProductCategoriesTableView,
      {
        name: new FormControl('', Validators.required)
      }
    );

    if (this.id) {
      this.productCategoryService.getProductCategoryDto(this.id,
        (result) => {
          if (result) {
            this.form.patchValue(result);
          }
        }
      );
    }
  }

  protected createItem(): void {
    this.productCategoryService.createProductCategory(
      <CreateProductCategoryCommand>
      {
        name: this.formName()
      },
      (result) => {
        this.exitForm();
      }
    );
  }

  protected updateItem(): void {
    this.productCategoryService.updateProductCategory(this.id,
      <UpdateProductCategoryCommand>
      {
        id: this.id,
        name: this.formName()
      },
      () => {
        this.exitForm();
      }
    );
  }
  //#endregion
}
