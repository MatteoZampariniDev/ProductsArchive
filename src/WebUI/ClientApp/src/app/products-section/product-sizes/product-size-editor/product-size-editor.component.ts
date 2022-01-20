import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EditorBaseComponent } from "../../../common/base-components/editor-base-component";
import { ProductsSectionRoutes } from "../../../common/enums/products-section-routes";
import { ProductSizeService } from "../../../common/services/products-section/product-size.service";
import { CreateProductSizeCommand, UpdateProductSizeCommand } from "../../../common/services/web-api-client";

@Component({
  selector: 'app-product-size-editor',
  templateUrl: './product-size-editor.component.html'
})
export class ProductSizeEditorComponent
  extends EditorBaseComponent implements OnInit {
  constructor(protected formBuilder: FormBuilder,
    protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected productSizeService: ProductSizeService) {
    super(formBuilder, activatedRoute, router);
  }

  //#region Internal
  protected formName(): string { return this.form.get("name").value; }

  ngOnInit(): void {
    this.getRouteParams();
    this.initializeComponent(
      this.id ? 'Modifica Formato' : 'Aggiungi Formato',
      this.id ? 'Modifica un formato esistente.' : 'Crea un nuovo formato.',
      ProductsSectionRoutes.ProductSizesTableView,
      {
        name: new FormControl('', Validators.required)
      }
    );

    if (this.id) {
      this.productSizeService.getProductSizeDto(this.id,
        (result) => {
          if (result) {
            this.form.patchValue(result);
          }
        }
      );
    }
  }

  protected createItem(): void {
    this.productSizeService.createProductSize(
      <CreateProductSizeCommand>
      {
        name: this.formName()
      },
      (result) => {
        this.exitForm();
      }
    );
  }

  protected updateItem(): void {
    this.productSizeService.updateProductSize(this.id,
      <UpdateProductSizeCommand>
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
