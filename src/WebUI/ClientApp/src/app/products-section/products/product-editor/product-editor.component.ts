import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EditorBaseComponent } from "../../../common/base-components/editor-base-component";
import { ProductsSectionRoutes } from "../../../common/enums/products-section-routes";
import { PaginationDetails } from "../../../common/models/pagination-details";
import { SortingDetails, SortingDirection } from "../../../common/models/sorting-details";
import { ProductGroupService } from "../../../common/services/products-section/product-group.service";
import { ProductService } from "../../../common/services/products-section/product.service";
import { ProductSizeService } from "../../../common/services/products-section/product-size.service";
import { CreateProductCommand, ProductGroupDto, ProductSizeDto, UpdateProductCommand } from "../../../common/services/web-api-client";


@Component({
  selector: 'app-product-editor',
  templateUrl: './product-editor.component.html'
})
export class ProductEditorComponent
  extends EditorBaseComponent implements OnInit {
  constructor(protected formBuilder: FormBuilder,
    protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected productService: ProductService,
    protected productGroupService: ProductGroupService,
    protected productSizeService: ProductSizeService) {
    super(formBuilder, activatedRoute, router);
  }

  //#region HTML
  public productGroups: ProductGroupDto[];
  public productSizes: ProductSizeDto[];
  //#endregion

  //#region Internal
  protected formProductId(): string { return this.form.get("productId").value; }
  protected formProductGroup(): ProductGroupDto { return this.form.get("group").value; }
  protected formProductSize(): ProductSizeDto { return this.form.get("size").value; }
  protected formNetWeight(): string { return this.form.get("netWeight").value; }

  ngOnInit(): void {
    this.getRouteParams();
    this.initializeComponent(
      this.id ? 'Modifica Prodotto' : 'Aggiungi Prodotto',
      this.id ? 'Modifica un prodotto esistente.' : 'Crea un nuovo prodotto.',
      ProductsSectionRoutes.ProductsTableView,
      {
        productId: new FormControl('', Validators.required),
        group: new FormControl('', Validators.required),
        size: new FormControl('', Validators.required),
        netWeight: new FormControl('', Validators.required)
      }
    );

    this.productGroupService.getPaginatedListOfProductGroupDto(
      new PaginationDetails(0, 999999), new SortingDetails('name', SortingDirection.ascending), null,
      (result) => {
        if (result) {
          this.productGroups = result.items;
        }
      }
    );

    this.productSizeService.getPaginatedListOfProductSizeDto(
      new PaginationDetails(0, 999999), new SortingDetails('name', SortingDirection.ascending), null,
      (result) => {
        if (result) {
          this.productSizes = result.items;
        }
      }
    );

    if (this.id) {
      this.productService.getProductDto(this.id,
        (result) => {
          if (result) {
            this.form.patchValue(result);
          }
        }
      );
    }
  }

  protected createItem(): void {
    this.productService.createProduct(
      <CreateProductCommand>
      {
        productId: this.formProductId(),
        groupId: this.formProductGroup().id,
        sizeId: this.formProductSize().id,
        netWeight: this.formNetWeight()
      },
      (result) => {
        this.exitForm();
      }
    );
  }

  protected updateItem(): void {
    this.productService.updateProduct(this.id,
      <UpdateProductCommand>
      {
        id: this.id,
        productId: this.formProductId(),
        groupId: this.formProductGroup().id,
        sizeId: this.formProductSize().id,
        netWeight: this.formNetWeight()
      },
      () => {
        this.exitForm();
      }
    );
  }
  //#endregion
}
