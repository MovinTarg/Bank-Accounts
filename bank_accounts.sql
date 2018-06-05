-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema bank_accounts
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema bank_accounts
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `bank_accounts` DEFAULT CHARACTER SET utf8 ;
USE `bank_accounts` ;

-- -----------------------------------------------------
-- Table `bank_accounts`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bank_accounts`.`users` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(255) NULL,
  `LastName` VARCHAR(255) NULL,
  `Email` VARCHAR(255) NULL,
  `Password` VARCHAR(255) NULL,
  `AccountBalance` FLOAT NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bank_accounts`.`transactions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bank_accounts`.`transactions` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Amount` FLOAT NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  `users_Id` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_transactions_users_idx` (`users_Id` ASC),
  CONSTRAINT `fk_transactions_users`
    FOREIGN KEY (`users_Id`)
    REFERENCES `bank_accounts`.`users` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
