import { OnInit } from "@angular/core";
import { Component } from "@angular/core";
import { FormControl } from "@angular/forms";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EditorBaseComponent } from "../../../common/base-components/editor-base-component";
import { ProductsSectionRoutes } from "../../../common/enums/products-section-routes";
import { PaginationDetails } from "../../../common/models/pagination-details";
import { SortingDetails, SortingDirection } from "../../../common/models/sorting-details";
import { ProductCategoryService } from "../../../common/services/products-section/product-category.service";
import { ProductGroupService } from "../../../common/services/products-section/product-group.service";
import { CreateProductGroupCommand, ProductCategoryDto, UpdateProductGroupCommand } from "../../../common/services/web-api-client";

@Component({
  selector: 'app-product-group-editor',
  templateUrl: './product-group-editor.component.html'
})
export class ProductGroupEditorComponent
  extends EditorBaseComponent implements OnInit {
  constructor(protected formBuilder: FormBuilder,
    protected activatedRoute: ActivatedRoute,
    protected router: Router,
    protected productGroupService: ProductGroupService,
    protected productCategoryService: ProductCategoryService) {
    super(formBuilder, activatedRoute, router);
  }

  //#region HTML
  public productCategories: ProductCategoryDto[];
  //#endregion

  //#region Internal
  protected formCategory(): ProductCategoryDto { return this.form.get("category").value; }
  protected formGroupId(): string { return this.form.get("groupId").value; }
  protected formName(): string { return this.form.get("name").value; }
  protected formDescription(): string { return this.form.get("description").value; }

  ngOnInit(): void {
    this.getRouteParams();
    this.initializeComponent(
      this.id ? 'Modifica Gruppo' : 'Aggiungi Gruppo',
      this.id ? 'Modifica un gruppo di prodotti esistente.' : 'Crea un nuovo gruppo.',
      ProductsSectionRoutes.ProductGroupsTableView,
      {
        category: new FormControl('', Validators.required),
        groupId: new FormControl('', Validators.required),
        name: new FormControl('', Validators.required),
        description: new FormControl('', Validators.required)
      }
    );

    this.productCategoryService.getPaginatedListOfProductCategoryDto(
      new PaginationDetails(0, 999999), new SortingDetails('name', SortingDirection.ascending), null,
      (result) => {
        if (result) {
          this.productCategories = result.items;
        }
      }
    );

    if (this.id) {
      this.productGroupService.getProductGroupDto(this.id,
        (result) => {
          if (result) {
            this.form.patchValue(result);
          }
        }
      );
    }
  }

  protected createItem(): void {
    this.productGroupService.createProductGroup(
      <CreateProductGroupCommand>
      {
        categoryId: this.formCategory().id,
        groupId: this.formGroupId(),
        name: this.formName(),
        description: this.formDescription()
      },
      (result) => {
        this.exitForm();
      }
    );
  }

  protected updateItem(): void {
    this.productGroupService.updateProductGroup(this.id,
      <UpdateProductGroupCommand>
      {
        id: this.id,
        categoryId: this.formCategory().id,
        groupId: this.formGroupId(),
        name: this.formName(),
        description: this.formDescription()
      },
      () => {
        this.exitForm();
      }
    );
  }
  //#endregion
}
