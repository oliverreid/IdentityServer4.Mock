terragrunt {
  # Configure Terragrunt to automatically store tfstate files in S3
  remote_state {
    backend = "s3"

    config {
      encrypt    = true
      bucket     = "dostuff-terraform-state"
      key        = "dostuff/prod/eu-wes-1/services/booking-api/state.tfstate"
      region     = "eu-west-1"
      lock_table = "terraform-euwest1"
    }
  }
}
